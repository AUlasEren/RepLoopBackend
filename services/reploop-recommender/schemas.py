from pydantic import BaseModel
from typing import Literal, Optional


class UserContext(BaseModel):
    user_id: str
    age: int
    weight_kg: float
    height_cm: float
    experience_level: Literal["Beginner", "Intermediate", "Advanced"]
    goal: Literal["WeightLoss", "MuscleGain", "Endurance", "Flexibility", "GeneralFitness"]


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


class RecommendationResponse(BaseModel):
    user_id: str
    algorithm: str
    recommendations: list[RecommendedWorkout]
