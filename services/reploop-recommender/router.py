import logging
import os

from fastapi import APIRouter, HTTPException, Query
from schemas import UserContext, RecommendationResponse, RecommendedWorkout
import engine
import database

logger = logging.getLogger(__name__)

router = APIRouter(prefix="/api/recommendations", tags=["recommendations"])

# DB baglantisi yoksa mock veri kullan
USE_MOCK = os.getenv("USE_MOCK_DB", "false").lower() == "true"


@router.post("/", response_model=RecommendationResponse)
def get_recommendations(
    user: UserContext,
    top_n: int = Query(default=5, ge=1, le=20),
):
    """
    Kullanici profiline gore weighted composite scoring ile antrenman onerileri dondur.

    - goal ve experience_level bilgisi zorunlu
    - Tamamlanmis workout'lar listeden cikarilmaz, variety/recency sinyalleri ile dengelenir
    - top_n: kac oneri donsun (varsayilan 5, max 20)
    """
    try:
        if USE_MOCK:
            workouts        = database.get_all_workouts_mock()
            session_history = database.get_session_history_mock(user.user_id)
        else:
            workouts        = database.get_all_workouts()
            session_history = database.get_session_history(user.user_id)
    except Exception as e:
        logger.error("DB connection failed: %s", e)
        raise HTTPException(status_code=503, detail="Database unavailable")

    raw_results = engine.recommend(
        user=user.model_dump(),
        workouts=workouts,
        top_n=top_n,
        session_history=session_history,
    )

    recommendations = [
        RecommendedWorkout(
            workout_id=r["id"],
            workout_name=r["name"],
            description=r.get("description"),
            duration_minutes=r.get("duration_minutes", 0),
            exercise_count=r.get("exercise_count", 0),
            muscle_groups=r.get("muscle_groups", []),
            score=r["score"],
            reason=r["reason"],
            tags=r["tags"],
        )
        for r in raw_results
    ]

    return RecommendationResponse(
        user_id=user.user_id,
        algorithm="composite-scoring-v3",
        recommendations=recommendations,
    )


@router.get("/health")
def health():
    return {"status": "ok", "service": "reploop-recommender"}
