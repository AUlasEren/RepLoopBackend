from pydantic import BaseModel
from typing import Literal, Optional


class UserContext(BaseModel):
    user_id: str
    age: int
    weight_kg: float
    height_cm: float
    experience_level: Literal["Beginner", "Intermediate", "Advanced"]
    goal: Literal["WeightLoss", "MuscleGain", "Endurance", "Flexibility", "GeneralFitness"]


class RecommendedExercise(BaseModel):
    exercise_id: str
    exercise_name: str
    sets: int
    reps: int
    weight_kg: float
    duration_seconds: int = 0


class RecommendedWorkout(BaseModel):
    workout_id: str
    workout_name: str
    description: Optional[str]
    duration_minutes: int
    exercise_count: int
    muscle_groups: list[str]
    score: float
    reason: str
    tags: list[str]
    exercises: list[RecommendedExercise] = []


class RecommendationResponse(BaseModel):
    user_id: str
    algorithm: str
    recommendations: list[RecommendedWorkout]


# ---------------------------------------------------------------------------
# Discovery endpoint modelleri
# ---------------------------------------------------------------------------

class DiscoverExercise(BaseModel):
    exercise_id: str
    name: str
    muscle_group: str
    equipment: str
    difficulty: str
    sets: int
    reps: int


class WorkoutTemplate(BaseModel):
    name: str
    description: str
    duration_minutes: int
    difficulty: str
    target_muscles: list[str]
    exercises: list[DiscoverExercise]
    score: float
    score_reasons: list[str]
    generated_by: str  # "llm" veya "algorithm"


class DiscoverResponse(BaseModel):
    templates: list[WorkoutTemplate]
