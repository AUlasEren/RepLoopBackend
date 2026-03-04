import os
import psycopg2
from psycopg2.extras import RealDictCursor
from dotenv import load_dotenv
from feature_extractor import infer_workout_features

load_dotenv()


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
    WorkoutDB -> workouts + workout_exercises
    ExerciseDB -> exercises (muscleGroup, difficulty)
    Python'da birlestirip infer_workout_features ile goal/level tahmin et.
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
                SELECT "WorkoutId"::text, "ExerciseId"::text
                FROM "WorkoutExercises"
            """)
            workout_exercises = cur.fetchall()
    finally:
        workout_conn.close()

    # workout_id -> [exercise_id, ...] map
    we_map: dict[str, list[str]] = {}
    for we in workout_exercises:
        wid = we["WorkoutId"]
        eid = we["ExerciseId"]
        we_map.setdefault(wid, []).append(eid)

    # 2. ExerciseDB: exercise detaylari
    exercise_conn = _get_exercise_conn()
    try:
        with exercise_conn.cursor(cursor_factory=RealDictCursor) as cur:
            cur.execute("""
                SELECT "Id"::text, "MuscleGroup", "Difficulty"
                FROM "Exercises"
            """)
            exercise_rows = cur.fetchall()
    finally:
        exercise_conn.close()

    # exercise_id -> {muscleGroup, difficulty} map
    exercise_map: dict[str, dict] = {}
    for e in exercise_rows:
        exercise_map[e["Id"]] = {
            "muscleGroup": e["MuscleGroup"],
            "difficulty": e["Difficulty"],
        }

    # 3. Python'da birlestir
    for workout in workouts:
        wid = workout["Id"]
        exercise_ids = we_map.get(wid, [])
        exercises = [exercise_map[eid] for eid in exercise_ids if eid in exercise_map]

        workout["exercise_count"] = len(exercise_ids)

        # EF Core PascalCase -> recommender snake_case field mapping
        workout["id"] = workout.pop("Id")
        workout["name"] = workout.pop("Name")
        workout["description"] = workout.pop("Description")
        workout["duration_minutes"] = workout.pop("DurationMinutes")

        inferred = infer_workout_features(exercises)
        workout["suitable_goal"] = inferred["suitable_goal"]
        workout["suitable_level"] = inferred["suitable_level"]

    return workouts


def get_completed_workout_ids(user_id: str) -> list[str]:
    """Kullanicinin tamamladigi session'lardaki workout ID'lerini dondur."""
    conn = _get_session_conn()
    try:
        with conn.cursor() as cur:
            cur.execute("""
                SELECT DISTINCT "WorkoutId"::text
                FROM "Sessions"
                WHERE "UserId" = %s::uuid AND "Status" = 'Completed'
            """, (user_id,))
            return [row[0] for row in cur.fetchall()]
    finally:
        conn.close()


# -- DB yokken test icin mock veri -------------------------------------------

MOCK_WORKOUTS = [
    {"id": "mock-1", "name": "Itme Gunu",     "description": "Gogus & Omuz & Triceps", "duration_minutes": 60, "exercise_count": 6, "suitable_goal": "MuscleGain",  "suitable_level": "Intermediate"},
    {"id": "mock-2", "name": "Cekme Gunu",    "description": "Sirt & Biceps",           "duration_minutes": 55, "exercise_count": 5, "suitable_goal": "MuscleGain",  "suitable_level": "Intermediate"},
    {"id": "mock-3", "name": "Bacak Gunu",    "description": "Quad & Hamstring & Glute","duration_minutes": 65, "exercise_count": 7, "suitable_goal": "MuscleGain",  "suitable_level": "Advanced"},
    {"id": "mock-4", "name": "HIIT Kardiyo",  "description": "Yuksek yogunluk interval","duration_minutes": 30, "exercise_count": 8, "suitable_goal": "Endurance",   "suitable_level": "Beginner"},
    {"id": "mock-5", "name": "Full Body",     "description": "Tum vucut programi",      "duration_minutes": 50, "exercise_count": 6, "suitable_goal": "GeneralFitness","suitable_level": "Beginner"},
    {"id": "mock-6", "name": "Yag Yakma",     "description": "Dusuk agirlik yuksek set","duration_minutes": 45, "exercise_count": 9, "suitable_goal": "WeightLoss",  "suitable_level": "Intermediate"},
]


def get_all_workouts_mock() -> list[dict]:
    return MOCK_WORKOUTS


def get_completed_workout_ids_mock(user_id: str) -> list[str]:
    return []
