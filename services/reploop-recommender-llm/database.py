from __future__ import annotations

import os
import logging
import threading
from contextlib import contextmanager
from datetime import datetime

from psycopg2.extras import RealDictCursor
from psycopg2.pool import ThreadedConnectionPool
from dotenv import load_dotenv

load_dotenv()
logger = logging.getLogger(__name__)

ZERO_UUID = "00000000-0000-0000-0000-000000000000"

# ---------------------------------------------------------------------------
# Connection pooling
# ---------------------------------------------------------------------------

_pools: dict[str, ThreadedConnectionPool] = {}
_pool_lock = threading.Lock()


def _get_pool(name: str, **kwargs) -> ThreadedConnectionPool:
    """Lazy-init thread-safe connection pool (double-checked locking)."""
    if name not in _pools:
        with _pool_lock:
            if name not in _pools:
                _pools[name] = ThreadedConnectionPool(
                    minconn=1, maxconn=5, **kwargs,
                )
    return _pools[name]


@contextmanager
def _workout_conn():
    pool = _get_pool(
        "workout",
        host=os.getenv("WORKOUT_DB_HOST", "localhost"),
        port=int(os.getenv("WORKOUT_DB_PORT", 5433)),
        dbname=os.getenv("WORKOUT_DB_NAME", "WorkoutDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )
    conn = pool.getconn()
    try:
        yield conn
    finally:
        pool.putconn(conn)


@contextmanager
def _exercise_conn():
    pool = _get_pool(
        "exercise",
        host=os.getenv("EXERCISE_DB_HOST", "localhost"),
        port=int(os.getenv("EXERCISE_DB_PORT", 5434)),
        dbname=os.getenv("EXERCISE_DB_NAME", "ExerciseDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )
    conn = pool.getconn()
    try:
        yield conn
    finally:
        pool.putconn(conn)


@contextmanager
def _user_conn():
    pool = _get_pool(
        "user",
        host=os.getenv("USER_DB_HOST", "localhost"),
        port=int(os.getenv("USER_DB_PORT", 5435)),
        dbname=os.getenv("USER_DB_NAME", "UserDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )
    conn = pool.getconn()
    try:
        yield conn
    finally:
        pool.putconn(conn)


@contextmanager
def _session_conn():
    pool = _get_pool(
        "session",
        host=os.getenv("SESSION_DB_HOST", "localhost"),
        port=int(os.getenv("SESSION_DB_PORT", 5437)),
        dbname=os.getenv("SESSION_DB_NAME", "SessionDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )
    conn = pool.getconn()
    try:
        yield conn
    finally:
        pool.putconn(conn)


def _normalize_name(name: str) -> str:
    """Bosluklari, tireleri kaldirip lowercase yap: 'Bench Press' -> 'benchpress'"""
    return name.replace(" ", "").replace("-", "").replace("_", "").lower()


def get_all_workouts(user_id: str) -> list[dict]:
    """
    Belirli kullanicinin workout'larini 3 ayri DB'den cekip birlestir.
    WorkoutDB  -> workouts + workout_exercises (ExerciseName, Sets, Reps, WeightKg)
    ExerciseDB -> exercises (MuscleGroup, Difficulty)  -- ExerciseId veya ExerciseName ile esle
    """
    # 1. WorkoutDB: kullanicinin workout'lari ve exercise iliskileri
    with _workout_conn() as w_conn:
        with w_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "Name", "Description", "DurationMinutes"
                FROM "Workouts"
                WHERE "UserId" = %s::uuid
            """, (user_id,))
            workouts = [dict(row) for row in cur.fetchall()]

            cur.execute("""
                SELECT we."WorkoutId"::text,
                       we."ExerciseId"::text,
                       we."ExerciseName",
                       we."Sets",
                       we."Reps",
                       we."WeightKg"
                FROM "WorkoutExercises" we
                JOIN "Workouts" w ON w."Id" = we."WorkoutId"
                WHERE w."UserId" = %s::uuid
            """, (user_id,))
            workout_exercises = [dict(row) for row in cur.fetchall()]

    # workout_id -> [workout_exercise_row, ...] map
    we_map: dict[str, list[dict]] = {}
    for we in workout_exercises:
        we_map.setdefault(we["WorkoutId"], []).append(we)

    # 2. ExerciseDB: exercise detaylari
    with _exercise_conn() as e_conn:
        with e_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "Name", "MuscleGroup", "Difficulty"
                FROM "Exercises"
            """)
            exercise_rows = [dict(row) for row in cur.fetchall()]

    # exercise_id -> detail map
    exercise_by_id: dict[str, dict] = {}
    # normalized exercise name -> detail map  (fallback icin)
    exercise_by_norm: dict[str, dict] = {}
    for e in exercise_rows:
        detail = {
            "name": e["Name"],
            "muscle_group": e["MuscleGroup"],
            "difficulty": e["Difficulty"],
        }
        exercise_by_id[e["Id"]] = detail
        if e["Name"]:
            exercise_by_norm[_normalize_name(e["Name"])] = detail

    # 3. Birlestir ve aggregate alanlar ekle
    for workout in workouts:
        wid = workout["Id"]
        we_rows = we_map.get(wid, [])

        total_sets = 0
        total_reps = 0
        has_weights = False
        muscle_groups: list[str] = []
        difficulties: list[str] = []

        for we in we_rows:
            eid = we["ExerciseId"]
            exercise_name = (we["ExerciseName"] or "").strip()

            # ExerciseId ile bul; zero UUID ise ExerciseName ile fallback
            detail = None
            if eid and eid != ZERO_UUID:
                detail = exercise_by_id.get(eid)

            # ID ile bulunamadiysa normalized name ile dene
            if not detail and exercise_name:
                norm = _normalize_name(exercise_name)
                detail = exercise_by_norm.get(norm)

                # Hala bulunamadiysa contains ile dene (orn. "Bench" -> "Bench Press")
                if not detail:
                    for db_norm, db_detail in exercise_by_norm.items():
                        if norm in db_norm or db_norm in norm:
                            detail = db_detail
                            break

            if detail:
                if detail["muscle_group"]:
                    muscle_groups.append(detail["muscle_group"])
                if detail["difficulty"]:
                    difficulties.append(detail["difficulty"])
            else:
                logger.debug(
                    "Exercise detail not found: id=%s name=%s", eid, exercise_name
                )

            total_sets += we.get("Sets") or 0
            total_reps += (we.get("Sets") or 0) * (we.get("Reps") or 0)
            if we.get("WeightKg") and float(we["WeightKg"]) > 0:
                has_weights = True

        # Build exercises list for embedding in response
        exercises = []
        for we in we_rows:
            eid = we["ExerciseId"]
            exercises.append({
                "exercise_id": eid if eid and eid != ZERO_UUID else "",
                "exercise_name": (we["ExerciseName"] or "").strip(),
                "sets": we.get("Sets") or 0,
                "reps": we.get("Reps") or 0,
                "weight_kg": float(we.get("WeightKg") or 0),
                "duration_seconds": 0,
            })

        # EF Core PascalCase -> recommender snake_case
        workout["id"] = workout.pop("Id")
        workout["name"] = workout.pop("Name")
        workout["description"] = workout.pop("Description")
        workout["duration_minutes"] = workout.pop("DurationMinutes")
        workout["exercise_count"] = len(we_rows)
        workout["total_sets"] = total_sets
        workout["total_reps"] = total_reps
        workout["has_weights"] = has_weights
        workout["muscle_groups"] = list(dict.fromkeys(muscle_groups))  # unique, order preserved
        workout["difficulties"] = difficulties
        workout["exercises"] = exercises

    return workouts


def get_session_history(user_id: str) -> list[dict]:
    """
    Kullanicinin tamamladigi session'lari tarih bilgisiyle dondur.
    Returns: [{"workout_id": str, "completed_at": datetime}, ...]
    """
    with _session_conn() as s_conn:
        with s_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "WorkoutId"::text AS workout_id,
                       "CompletedAt"     AS completed_at
                FROM "Sessions"
                WHERE "UserId" = %s::uuid
                  AND "Status" = 'Completed'
                  AND "CompletedAt" IS NOT NULL
                ORDER BY "CompletedAt" DESC
            """, (user_id,))
            return [dict(row) for row in cur.fetchall()]


# ---------------------------------------------------------------------------
# Discovery endpoint icin query fonksiyonlari
# ---------------------------------------------------------------------------

def get_user_profile(user_id: str) -> dict:
    """UserDB'den kullanici profilini cek. Bulunamazsa default degerler."""
    try:
        with _user_conn() as u_conn:
            with u_conn.cursor(cursor_factory=RealDictCursor) as cur:
                cur.execute("""
                    SELECT "ExperienceLevel", "Goal"
                    FROM "UserProfiles"
                    WHERE "UserId" = %s::uuid
                """, (user_id,))
                row = cur.fetchone()
    except Exception as e:
        logger.warning("UserDB query failed: %s", e)
        row = None

    if row:
        return {
            "experience_level": row["ExperienceLevel"] or "Beginner",
            "goal": row["Goal"] or "GeneralFitness",
        }
    return {"experience_level": "Beginner", "goal": "GeneralFitness"}


def get_muscle_frequency(user_id: str) -> tuple[dict[str, int], set[str]]:
    """
    Son 30 gunde tamamlanan session'lardaki exercise frekansini hesapla.
    SessionDB (set sayisi) + ExerciseDB (MuscleGroup, Equipment) cross-DB lookup.
    Returns: (muscle_frequency, equipment_used)
    """
    # Adim 1: SessionDB — exercise bazinda set sayisi
    with _session_conn() as s_conn:
        with s_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT ss."ExerciseId"::text AS exercise_id, COUNT(*) AS set_count
                FROM "Sets" ss
                JOIN "Sessions" s ON s."Id" = ss."SessionId"
                WHERE s."UserId" = %s::uuid
                  AND s."Status" = 'Completed'
                  AND s."CompletedAt" >= NOW() - INTERVAL '30 days'
                GROUP BY ss."ExerciseId"
            """, (user_id,))
            exercise_freq = {row["exercise_id"]: row["set_count"] for row in cur.fetchall()}

    if not exercise_freq:
        return {}, set()

    # Adim 2: ExerciseDB — MuscleGroup + Equipment lookup
    exercise_ids = list(exercise_freq.keys())
    muscle_frequency: dict[str, int] = {}
    equipment_used: set[str] = set()

    with _exercise_conn() as e_conn:
        with e_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "MuscleGroup", "Equipment"
                FROM "Exercises"
                WHERE "Id" = ANY(%s::uuid[])
            """, (exercise_ids,))

            for row in cur.fetchall():
                eid = row["Id"]
                mg = row["MuscleGroup"]
                eq = row["Equipment"]
                count = exercise_freq.get(eid, 0)

                if mg:
                    muscle_frequency[mg] = muscle_frequency.get(mg, 0) + count
                if eq:
                    equipment_used.add(eq)

    return muscle_frequency, equipment_used


def get_user_workout_exercise_ids(user_id: str) -> set[str]:
    """Kullanicinin mevcut workout'larindaki exercise ID'lerini dondur (overlap tespiti)."""
    with _workout_conn() as w_conn:
        with w_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT DISTINCT we."ExerciseId"::text
                FROM "WorkoutExercises" we
                JOIN "Workouts" w ON w."Id" = we."WorkoutId"
                WHERE w."UserId" = %s::uuid
            """, (user_id,))
            return {row["ExerciseId"] for row in cur.fetchall()}


def get_public_exercises() -> list[dict]:
    """Tum public exercise'leri ExerciseDB'den cek (metadata'si tam olanlar)."""
    with _exercise_conn() as e_conn:
        with e_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "Name", "MuscleGroup", "Equipment", "Difficulty"
                FROM "Exercises"
                WHERE "IsPublic" = true
                  AND "MuscleGroup" IS NOT NULL
                  AND "Difficulty" IS NOT NULL
            """)
            return [dict(row) for row in cur.fetchall()]


# -- Mock veri (DB yokken test icin) ------------------------------------------

MOCK_WORKOUTS = [
    {
        "id": "mock-1", "name": "Itme Gunu",
        "description": "Gogus & Omuz & Triceps",
        "duration_minutes": 60, "exercise_count": 6,
        "total_sets": 18, "total_reps": 180, "has_weights": True,
        "muscle_groups": ["Chest", "Shoulders", "Triceps"],
        "difficulties": ["Intermediate", "Intermediate", "Intermediate",
                         "Intermediate", "Beginner", "Beginner"],
        "exercises": [
            {"exercise_id": "m1-e1", "exercise_name": "Bench Press", "sets": 3, "reps": 10, "weight_kg": 60, "duration_seconds": 0},
            {"exercise_id": "m1-e2", "exercise_name": "Overhead Press", "sets": 3, "reps": 10, "weight_kg": 30, "duration_seconds": 0},
            {"exercise_id": "m1-e3", "exercise_name": "Incline Dumbbell Press", "sets": 3, "reps": 10, "weight_kg": 20, "duration_seconds": 0},
            {"exercise_id": "m1-e4", "exercise_name": "Lateral Raise", "sets": 3, "reps": 12, "weight_kg": 8, "duration_seconds": 0},
            {"exercise_id": "m1-e5", "exercise_name": "Triceps Pushdown", "sets": 3, "reps": 12, "weight_kg": 15, "duration_seconds": 0},
            {"exercise_id": "m1-e6", "exercise_name": "Dips", "sets": 3, "reps": 10, "weight_kg": 0, "duration_seconds": 0},
        ],
    },
    {
        "id": "mock-2", "name": "Cekme Gunu",
        "description": "Sirt & Biceps",
        "duration_minutes": 55, "exercise_count": 5,
        "total_sets": 15, "total_reps": 150, "has_weights": True,
        "muscle_groups": ["Back", "Biceps"],
        "difficulties": ["Intermediate", "Intermediate", "Intermediate",
                         "Advanced", "Beginner"],
        "exercises": [
            {"exercise_id": "m2-e1", "exercise_name": "Deadlift", "sets": 3, "reps": 8, "weight_kg": 80, "duration_seconds": 0},
            {"exercise_id": "m2-e2", "exercise_name": "Barbell Row", "sets": 3, "reps": 10, "weight_kg": 50, "duration_seconds": 0},
            {"exercise_id": "m2-e3", "exercise_name": "Lat Pulldown", "sets": 3, "reps": 12, "weight_kg": 40, "duration_seconds": 0},
            {"exercise_id": "m2-e4", "exercise_name": "Pull-Up", "sets": 3, "reps": 8, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m2-e5", "exercise_name": "Bicep Curl", "sets": 3, "reps": 12, "weight_kg": 12, "duration_seconds": 0},
        ],
    },
    {
        "id": "mock-3", "name": "Bacak Gunu",
        "description": "Quad & Hamstring & Glute",
        "duration_minutes": 65, "exercise_count": 7,
        "total_sets": 24, "total_reps": 240, "has_weights": True,
        "muscle_groups": ["Quadriceps", "Hamstrings", "Glutes", "Calves"],
        "difficulties": ["Advanced", "Advanced", "Advanced",
                         "Intermediate", "Intermediate", "Intermediate", "Advanced"],
        "exercises": [
            {"exercise_id": "m3-e1", "exercise_name": "Squat", "sets": 4, "reps": 8, "weight_kg": 80, "duration_seconds": 0},
            {"exercise_id": "m3-e2", "exercise_name": "Romanian Deadlift", "sets": 3, "reps": 10, "weight_kg": 60, "duration_seconds": 0},
            {"exercise_id": "m3-e3", "exercise_name": "Leg Press", "sets": 4, "reps": 10, "weight_kg": 120, "duration_seconds": 0},
            {"exercise_id": "m3-e4", "exercise_name": "Leg Curl", "sets": 3, "reps": 12, "weight_kg": 30, "duration_seconds": 0},
            {"exercise_id": "m3-e5", "exercise_name": "Leg Extension", "sets": 3, "reps": 12, "weight_kg": 35, "duration_seconds": 0},
            {"exercise_id": "m3-e6", "exercise_name": "Hip Thrust", "sets": 3, "reps": 10, "weight_kg": 60, "duration_seconds": 0},
            {"exercise_id": "m3-e7", "exercise_name": "Calf Raise", "sets": 4, "reps": 15, "weight_kg": 40, "duration_seconds": 0},
        ],
    },
    {
        "id": "mock-4", "name": "HIIT Kardiyo",
        "description": "Yuksek yogunluk interval",
        "duration_minutes": 30, "exercise_count": 8,
        "total_sets": 24, "total_reps": 360, "has_weights": False,
        "muscle_groups": ["Cardio", "Core"],
        "difficulties": ["Beginner", "Beginner", "Beginner", "Beginner",
                         "Beginner", "Beginner", "Intermediate", "Intermediate"],
        "exercises": [
            {"exercise_id": "m4-e1", "exercise_name": "Jumping Jack", "sets": 3, "reps": 20, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e2", "exercise_name": "Burpee", "sets": 3, "reps": 10, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e3", "exercise_name": "Mountain Climber", "sets": 3, "reps": 20, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e4", "exercise_name": "High Knees", "sets": 3, "reps": 20, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e5", "exercise_name": "Jump Squat", "sets": 3, "reps": 15, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e6", "exercise_name": "Plank", "sets": 3, "reps": 1, "weight_kg": 0, "duration_seconds": 30},
            {"exercise_id": "m4-e7", "exercise_name": "Bicycle Crunch", "sets": 3, "reps": 20, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m4-e8", "exercise_name": "Box Jump", "sets": 3, "reps": 10, "weight_kg": 0, "duration_seconds": 0},
        ],
    },
    {
        "id": "mock-5", "name": "Full Body",
        "description": "Tum vucut programi",
        "duration_minutes": 50, "exercise_count": 6,
        "total_sets": 18, "total_reps": 180, "has_weights": True,
        "muscle_groups": ["Chest", "Back", "Shoulders", "Quadriceps", "Core"],
        "difficulties": ["Beginner", "Beginner", "Beginner",
                         "Beginner", "Beginner", "Beginner"],
        "exercises": [
            {"exercise_id": "m5-e1", "exercise_name": "Push-Up", "sets": 3, "reps": 15, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m5-e2", "exercise_name": "Dumbbell Row", "sets": 3, "reps": 12, "weight_kg": 15, "duration_seconds": 0},
            {"exercise_id": "m5-e3", "exercise_name": "Shoulder Press", "sets": 3, "reps": 10, "weight_kg": 12, "duration_seconds": 0},
            {"exercise_id": "m5-e4", "exercise_name": "Goblet Squat", "sets": 3, "reps": 12, "weight_kg": 16, "duration_seconds": 0},
            {"exercise_id": "m5-e5", "exercise_name": "Plank", "sets": 3, "reps": 1, "weight_kg": 0, "duration_seconds": 30},
            {"exercise_id": "m5-e6", "exercise_name": "Lunges", "sets": 3, "reps": 10, "weight_kg": 10, "duration_seconds": 0},
        ],
    },
    {
        "id": "mock-6", "name": "Yag Yakma",
        "description": "Dusuk agirlik yuksek set",
        "duration_minutes": 45, "exercise_count": 9,
        "total_sets": 27, "total_reps": 405, "has_weights": True,
        "muscle_groups": ["Chest", "Back", "Shoulders", "Quadriceps", "Core", "Cardio"],
        "difficulties": ["Intermediate", "Intermediate", "Intermediate",
                         "Intermediate", "Beginner", "Beginner",
                         "Beginner", "Beginner", "Beginner"],
        "exercises": [
            {"exercise_id": "m6-e1", "exercise_name": "Kettlebell Swing", "sets": 3, "reps": 15, "weight_kg": 16, "duration_seconds": 0},
            {"exercise_id": "m6-e2", "exercise_name": "Dumbbell Snatch", "sets": 3, "reps": 10, "weight_kg": 12, "duration_seconds": 0},
            {"exercise_id": "m6-e3", "exercise_name": "Push-Up", "sets": 3, "reps": 15, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m6-e4", "exercise_name": "Renegade Row", "sets": 3, "reps": 10, "weight_kg": 10, "duration_seconds": 0},
            {"exercise_id": "m6-e5", "exercise_name": "Thrusters", "sets": 3, "reps": 12, "weight_kg": 15, "duration_seconds": 0},
            {"exercise_id": "m6-e6", "exercise_name": "Box Jump", "sets": 3, "reps": 10, "weight_kg": 0, "duration_seconds": 0},
            {"exercise_id": "m6-e7", "exercise_name": "Battle Rope", "sets": 3, "reps": 1, "weight_kg": 0, "duration_seconds": 30},
            {"exercise_id": "m6-e8", "exercise_name": "Plank", "sets": 3, "reps": 1, "weight_kg": 0, "duration_seconds": 30},
            {"exercise_id": "m6-e9", "exercise_name": "Jumping Lunges", "sets": 3, "reps": 12, "weight_kg": 0, "duration_seconds": 0},
        ],
    },
]


def get_all_workouts_mock() -> list[dict]:
    return MOCK_WORKOUTS


def get_session_history_mock(user_id: str) -> list[dict]:
    return [
        {"workout_id": "mock-1", "completed_at": datetime(2026, 2, 28, 10, 0)},
        {"workout_id": "mock-4", "completed_at": datetime(2026, 2, 25, 8, 30)},
    ]
