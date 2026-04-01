import logging
import os
import threading

from fastapi import APIRouter, HTTPException, Query
from schemas import (
    UserContext, RecommendationResponse, RecommendedWorkout, RecommendedExercise,
    DiscoverResponse, WorkoutTemplate, DiscoverExercise,
)
import engine2 as engine
import cache
import discover_engine
import discover_llm

logger = logging.getLogger(__name__)

router = APIRouter(prefix="/api/recommendations", tags=["recommendations"])

USE_MOCK = os.getenv("USE_MOCK_DB", "false").lower() == "true"


@router.post("/", response_model=RecommendationResponse)
def get_recommendations(
    user: UserContext,
    top_n: int = Query(default=5, ge=1, le=20),
):
    """
    Kullanici profiline gore LLM-destekli antrenman onerileri dondur.
    LLM basarisiz olursa algorithmic scoring'e fallback yapar.
    """
    try:
        workouts = cache.get_all_workouts(user.user_id, use_mock=USE_MOCK)
        session_history = cache.get_session_history(user.user_id, use_mock=USE_MOCK)
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
            exercises=[
                RecommendedExercise(**ex)
                for ex in r.get("exercises", [])
            ],
        )
        for r in raw_results
    ]

    return RecommendationResponse(
        user_id=user.user_id,
        algorithm="llm-gemma2-9b",
        recommendations=recommendations,
    )


@router.get("/discover", response_model=DiscoverResponse)
def discover_templates(
    user_id: str = Query(..., description="Kullanici UUID"),
):
    """
    Async enrichment pipeline:
    1. LLM cache varsa hemen don (generated_by: "llm")
    2. Yoksa algoritmik sonucu hemen don (generated_by: "algorithm")
    3. Arka planda LLM calistir, bitince cache'e yaz
    4. Sonraki istekte LLM sonuclari gelir
    """
    # 1. Kullanici verileri
    profile_raw = cache.get_user_profile(user_id)
    muscle_freq, equipment_used = cache.get_muscle_frequency(user_id)
    workout_exercise_ids = cache.get_user_workout_exercise_ids(user_id)

    # 2. Cache key: user_id + program hash (workout degisince miss olur)
    cache_key = cache.discover_cache_key(user_id, workout_exercise_ids)

    # 3. LLM cache check — varsa hemen don
    cached_templates = cache.get_llm_templates(cache_key)
    if cached_templates is not None:
        logger.info("Returning cached LLM templates for %s", user_id)
        return DiscoverResponse(templates=_to_response(cached_templates))

    # 4. User profile + exercises
    user_profile = {
        "experience_level": profile_raw.get("experience_level", "Beginner"),
        "goal": profile_raw.get("goal", "GeneralFitness"),
        "muscle_frequency": muscle_freq,
        "equipment_used": equipment_used,
        "workout_exercise_ids": workout_exercise_ids,
    }

    try:
        exercises = cache.get_public_exercises()
    except Exception as e:
        logger.error("ExerciseDB connection failed: %s", e)
        raise HTTPException(status_code=503, detail="ExerciseDB unavailable")

    if not exercises:
        return DiscoverResponse(templates=[])

    # 5. Algoritmik skorlama (hizli, <100ms)
    candidates = discover_engine.filter_top_candidates(exercises, user_profile, top_n=15)
    algo_templates = discover_engine.generate_templates_fallback(
        candidates, user_profile, count=3,
    )

    # 6. Arka planda LLM enrichment baslat (zaten calismiyorsa)
    if not cache.is_llm_in_progress(cache_key):
        cache.mark_llm_in_progress(cache_key, True)
        threading.Thread(
            target=_enrich_with_llm,
            args=(cache_key, candidates, user_profile),
            daemon=True,
        ).start()

    # 7. Algoritmik sonucu hemen don
    return DiscoverResponse(templates=_to_response(algo_templates))


def _to_response(templates: list[dict]) -> list[WorkoutTemplate]:
    """Template dict listesini response modeline donustur."""
    return [
        WorkoutTemplate(
            name=t["name"],
            description=t["description"],
            duration_minutes=t["duration_minutes"],
            difficulty=t["difficulty"],
            target_muscles=t["target_muscles"],
            exercises=[DiscoverExercise(**ex) for ex in t["exercises"]],
            score=t["score"],
            score_reasons=t["score_reasons"],
            generated_by=t["generated_by"],
        )
        for t in templates
    ]


def _enrich_with_llm(cache_key: str, candidates: list[dict], user_profile: dict):
    """Arka planda LLM ile template uret, bitince cache'e yaz."""
    try:
        templates = discover_llm.generate_templates_with_llm(candidates, user_profile)
        if templates:
            cache.set_llm_templates(cache_key, templates)
            logger.info("LLM enrichment complete (%s): %d templates", cache_key, len(templates))
        else:
            logger.info("LLM enrichment returned None (%s)", cache_key)
    except Exception as e:
        logger.error("LLM enrichment failed (%s): %s", cache_key, e)
    finally:
        cache.mark_llm_in_progress(cache_key, False)


@router.get("/health")
def health():
    return {"status": "ok", "service": "reploop-recommender-llm"}
