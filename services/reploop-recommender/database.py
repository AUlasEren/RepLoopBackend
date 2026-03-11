import os
import logging
from datetime import datetime

import psycopg2
from psycopg2.extras import RealDictCursor
from dotenv import load_dotenv

load_dotenv()
logger = logging.getLogger(__name__)

ZERO_UUID = "00000000-0000-0000-0000-000000000000"


def _normalize_name(name: str) -> str:
    """Bosluklari, tireleri kaldirip lowercase yap: 'Bench Press' -> 'benchpress'"""
    return name.replace(" ", "").replace("-", "").replace("_", "").lower()


def _get_workout_conn():
    return psycopg2.connect(
        host=os.getenv("WORKOUT_DB_HOST", "localhost"),
        port=int(os.getenv("WORKOUT_DB_PORT", 5433)),
        dbname=os.getenv("WORKOUT_DB_NAME", "WorkoutDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )


def _get_exercise_conn():
    return psycopg2.connect(
        host=os.getenv("EXERCISE_DB_HOST", "localhost"),
        port=int(os.getenv("EXERCISE_DB_PORT", 5434)),
        dbname=os.getenv("EXERCISE_DB_NAME", "ExerciseDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )


def _get_session_conn():
    return psycopg2.connect(
        host=os.getenv("SESSION_DB_HOST", "localhost"),
        port=int(os.getenv("SESSION_DB_PORT", 5437)),
        dbname=os.getenv("SESSION_DB_NAME", "SessionDB"),
        user=os.getenv("DB_USER", "reploop"),
        password=os.getenv("DB_PASSWORD", "reploop123"),
    )


def get_all_workouts() -> list[dict]:
    """
    Tum workout'lari 3 ayri DB'den cekip birlestir.
    WorkoutDB  -> workouts + workout_exercises (ExerciseName, Sets, Reps, WeightKg)
    ExerciseDB -> exercises (MuscleGroup, Difficulty)  -- ExerciseId veya ExerciseName ile esle
    """
    # 1. WorkoutDB: workout'lar ve workout-exercise iliskileri
    workout_conn = _get_workout_conn()
    try:
        with workout_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "Name", "Description", "DurationMinutes"
                FROM "Workouts"
            """)
            workouts = [dict(row) for row in cur.fetchall()]

            cur.execute("""
                SELECT "WorkoutId"::text,
                       "ExerciseId"::text,
                       "ExerciseName",
                       "Sets",
                       "Reps",
                       "WeightKg"
                FROM "WorkoutExercises"
            """)
            workout_exercises = [dict(row) for row in cur.fetchall()]
    finally:
        workout_conn.close()

    # workout_id -> [workout_exercise_row, ...] map
    we_map: dict[str, list[dict]] = {}
    for we in workout_exercises:
        we_map.setdefault(we["WorkoutId"], []).append(we)

    # 2. ExerciseDB: exercise detaylari
    exercise_conn = _get_exercise_conn()
    try:
        with exercise_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "Name", "MuscleGroup", "Difficulty"
                FROM "Exercises"
            """)
            exercise_rows = [dict(row) for row in cur.fetchall()]
    finally:
        exercise_conn.close()

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

    return workouts


def get_session_history(user_id: str) -> list[dict]:
    """
    Kullanicinin tamamladigi session'lari tarih bilgisiyle dondur.
    Returns: [{"workout_id": str, "completed_at": datetime}, ...]
    """
    conn = _get_session_conn()
    try:
        with conn.cursor(cursor_factory=RealDictCursor) as cur:
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
    finally:
        conn.close()


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
    },
    {
        "id": "mock-2", "name": "Cekme Gunu",
        "description": "Sirt & Biceps",
        "duration_minutes": 55, "exercise_count": 5,
        "total_sets": 15, "total_reps": 150, "has_weights": True,
        "muscle_groups": ["Back", "Biceps"],
        "difficulties": ["Intermediate", "Intermediate", "Intermediate",
                         "Advanced", "Beginner"],
    },
    {
        "id": "mock-3", "name": "Bacak Gunu",
        "description": "Quad & Hamstring & Glute",
        "duration_minutes": 65, "exercise_count": 7,
        "total_sets": 24, "total_reps": 240, "has_weights": True,
        "muscle_groups": ["Quadriceps", "Hamstrings", "Glutes", "Calves"],
        "difficulties": ["Advanced", "Advanced", "Advanced",
                         "Intermediate", "Intermediate", "Intermediate", "Advanced"],
    },
    {
        "id": "mock-4", "name": "HIIT Kardiyo",
        "description": "Yuksek yogunluk interval",
        "duration_minutes": 30, "exercise_count": 8,
        "total_sets": 24, "total_reps": 360, "has_weights": False,
        "muscle_groups": ["Cardio", "Core"],
        "difficulties": ["Beginner", "Beginner", "Beginner", "Beginner",
                         "Beginner", "Beginner", "Intermediate", "Intermediate"],
    },
    {
        "id": "mock-5", "name": "Full Body",
        "description": "Tum vucut programi",
        "duration_minutes": 50, "exercise_count": 6,
        "total_sets": 18, "total_reps": 180, "has_weights": True,
        "muscle_groups": ["Chest", "Back", "Shoulders", "Quadriceps", "Core"],
        "difficulties": ["Beginner", "Beginner", "Beginner",
                         "Beginner", "Beginner", "Beginner"],
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
    },
]


def get_all_workouts_mock() -> list[dict]:
    return MOCK_WORKOUTS


def get_session_history_mock(user_id: str) -> list[dict]:
    return [
        {"workout_id": "mock-1", "completed_at": datetime(2026, 2, 28, 10, 0)},
        {"workout_id": "mock-4", "completed_at": datetime(2026, 2, 25, 8, 30)},
    ]
