"""
discover_engine.py — Algoritmik scoring + fallback template generation.

Katman 1: 117 public exercise'i skorla, top 15'i filtrele.
Fallback: LLM basarisiz olursa rule-based template olustur.
"""
from __future__ import annotations

import logging

logger = logging.getLogger(__name__)

# ---------------------------------------------------------------------------
# Sabitler
# ---------------------------------------------------------------------------

GOAL_MUSCLE_MAP = {
    "MuscleGain": {
        "primary": ["Chest", "Back", "Legs", "Shoulders"],
        "secondary": ["Biceps", "Triceps", "Core"],
    },
    "WeightLoss": {
        "primary": ["Cardio", "Full Body", "Legs"],
        "secondary": ["Core"],
    },
    "Endurance": {
        "primary": ["Cardio", "Full Body"],
        "secondary": ["Legs", "Core"],
    },
    "Flexibility": {
        "primary": ["Full Body", "Core"],
        "secondary": ["Legs", "Shoulders"],
    },
    "GeneralFitness": {
        "primary": ["Full Body", "Cardio", "Legs"],
        "secondary": ["Chest", "Back", "Core"],
    },
}

ANTAGONIST_MAP = {
    "Chest": "Back",
    "Back": "Chest",
    "Biceps": "Triceps",
    "Triceps": "Biceps",
    "Shoulders": "Core",
    "Legs": "Core",
    "Core": "Legs",
    "Cardio": "Full Body",
    "Full Body": "Cardio",
}

SETS_REPS_BY_LEVEL = {
    "Beginner": (3, 12),
    "Intermediate": (4, 10),
    "Advanced": (5, 6),
}

DIFF_ORDER = {"Beginner": 0, "Intermediate": 1, "Advanced": 2}

_MUSCLE_TR = {
    "Chest": "Göğüs",
    "Back": "Sırt",
    "Legs": "Bacak",
    "Shoulders": "Omuz",
    "Biceps": "Biceps",
    "Triceps": "Triceps",
    "Core": "Core",
    "Cardio": "Kardiyo",
    "Full Body": "Tam Vücut",
}

_GOAL_TR = {
    "MuscleGain": "Kas kazanımı",
    "WeightLoss": "Kilo verme",
    "Endurance": "Dayanıklılık",
    "Flexibility": "Esneklik",
    "GeneralFitness": "Genel fitness",
}

_LEVEL_TR = {
    "Beginner": "başlangıç",
    "Intermediate": "orta",
    "Advanced": "ileri",
}


# ---------------------------------------------------------------------------
# Exercise scoring
# ---------------------------------------------------------------------------

def score_exercise(exercise: dict, user_profile: dict) -> float:
    """
    5 sinyal ile composite score.
    user_profile keys: muscle_frequency, equipment_used, experience_level, goal,
                       workout_exercise_ids
    """
    muscle = exercise.get("MuscleGroup", "")

    # 1. Novelty (0.25) — az calisilan kas gruplari daha yuksek skor
    mf = user_profile.get("muscle_frequency", {})
    max_freq = max(mf.values()) if mf else 1
    novelty = 1.0 - (mf.get(muscle, 0) / max(max_freq, 1))

    # 2. Goal alignment (0.25) — hedefe uygun kas grubu mu
    goal = user_profile.get("goal", "GeneralFitness")
    goal_muscles = GOAL_MUSCLE_MAP.get(goal, GOAL_MUSCLE_MAP["GeneralFitness"])
    if muscle in goal_muscles["primary"]:
        goal_score = 1.0
    elif muscle in goal_muscles["secondary"]:
        goal_score = 0.6
    else:
        goal_score = 0.2

    # 3. Difficulty match (0.20) — seviyeye uygun mu
    user_level = DIFF_ORDER.get(user_profile.get("experience_level", "Beginner"), 0)
    ex_level = DIFF_ORDER.get(exercise.get("Difficulty", "Beginner"), 0)
    diff_delta = abs(user_level - ex_level)
    difficulty_score = 1.0 if diff_delta == 0 else 0.5 if diff_delta == 1 else 0.1

    # 4. Equipment familiarity (0.10) — tanidik ekipman bonus
    equipment = exercise.get("Equipment", "")
    equipment_used = user_profile.get("equipment_used", set())
    if equipment == "Bodyweight":
        equip_score = 0.9
    elif equipment in equipment_used:
        equip_score = 0.8
    else:
        equip_score = 0.5

    # 5. Workout overlap (0.20) — zaten programda olan exercise'e ceza
    workout_ex_ids = user_profile.get("workout_exercise_ids", set())
    overlap_score = 0.1 if exercise.get("Id", "") in workout_ex_ids else 1.0

    return round(
        novelty * 0.25
        + goal_score * 0.25
        + difficulty_score * 0.20
        + equip_score * 0.10
        + overlap_score * 0.20,
        3,
    )


# ---------------------------------------------------------------------------
# Filtering — top N candidates (kas grubu cesitliligi koruyarak)
# ---------------------------------------------------------------------------

def filter_top_candidates(
    exercises: list[dict],
    user_profile: dict,
    top_n: int = 15,
) -> list[dict]:
    """Tum exercise'leri skorla, top_n'i sec. Her kas grubundan en az 1 garanti."""
    for ex in exercises:
        ex["_score"] = score_exercise(ex, user_profile)

    # Her kas grubundan en iyi 1'i garanti et
    by_muscle: dict[str, dict] = {}
    for ex in exercises:
        mg = ex.get("MuscleGroup", "")
        if mg not in by_muscle or ex["_score"] > by_muscle[mg]["_score"]:
            by_muscle[mg] = ex

    guaranteed = list(by_muscle.values())
    guaranteed_ids = {e["Id"] for e in guaranteed}

    remaining = [e for e in exercises if e["Id"] not in guaranteed_ids]
    remaining.sort(key=lambda e: e["_score"], reverse=True)

    result = guaranteed + remaining[: max(0, top_n - len(guaranteed))]
    result.sort(key=lambda e: e["_score"], reverse=True)
    return result[:top_n]


# ---------------------------------------------------------------------------
# Fallback template generation (LLM basarisiz oldugunda)
# ---------------------------------------------------------------------------

def generate_templates_fallback(
    candidates: list[dict],
    user_profile: dict,
    count: int = 3,
) -> list[dict]:
    """Rule-based template generation. LLM fallback'i olarak kullanilir."""
    goal = user_profile.get("goal", "GeneralFitness")
    experience = user_profile.get("experience_level", "Beginner")
    goal_muscles = GOAL_MUSCLE_MAP.get(goal, GOAL_MUSCLE_MAP["GeneralFitness"])
    primary_targets = goal_muscles["primary"]
    sets, reps = SETS_REPS_BY_LEVEL.get(experience, (3, 12))

    templates: list[dict] = []
    used_ids: set[str] = set()

    for i in range(count):
        target = primary_targets[i % len(primary_targets)]
        complement = ANTAGONIST_MAP.get(target, "Core")

        main = _top_n_for_muscle(candidates, target, 3, used_ids)
        comp = _top_n_for_muscle(candidates, complement, 1, used_ids)
        selected = main + comp

        if not selected:
            continue

        used_ids.update(e["Id"] for e in selected)
        avg_score = sum(e["_score"] for e in selected) / len(selected)

        target_muscles = list(dict.fromkeys(
            [e.get("MuscleGroup") for e in selected if e.get("MuscleGroup")]
        ))

        muscle_tr = _MUSCLE_TR.get(target, target)
        level_tr = _LEVEL_TR.get(experience, experience)

        templates.append({
            "name": f"{muscle_tr} Odaklı Antrenman",
            "description": f"{', '.join(target_muscles)} hedefli, {level_tr} seviye program",
            "duration_minutes": len(selected) * 8 + 5,
            "difficulty": experience,
            "target_muscles": target_muscles,
            "exercises": [
                {
                    "exercise_id": e["Id"],
                    "name": e["Name"],
                    "muscle_group": e.get("MuscleGroup", ""),
                    "equipment": e.get("Equipment", ""),
                    "difficulty": e.get("Difficulty", ""),
                    "sets": sets,
                    "reps": reps,
                }
                for e in selected
            ],
            "score": round(avg_score, 2),
            "score_reasons": _make_reasons(target, user_profile),
            "generated_by": "algorithm",
        })

    templates.sort(key=lambda t: t["score"], reverse=True)
    return templates


# ---------------------------------------------------------------------------
# Yardimci fonksiyonlar
# ---------------------------------------------------------------------------

def _top_n_for_muscle(
    candidates: list[dict],
    muscle_group: str,
    n: int,
    exclude_ids: set[str],
) -> list[dict]:
    """Belirli kas grubundan en yuksek skorlu n exercise sec."""
    result: list[dict] = []
    for ex in candidates:
        if ex["Id"] in exclude_ids:
            continue
        if ex.get("MuscleGroup") == muscle_group:
            result.append(ex)
            if len(result) >= n:
                break
    return result


def _make_reasons(target_muscle: str, profile: dict) -> list[str]:
    """Template icin skor aciklamalari olustur."""
    reasons: list[str] = []
    freq = profile.get("muscle_frequency", {})
    muscle_tr = _MUSCLE_TR.get(target_muscle, target_muscle)

    if freq.get(target_muscle, 0) == 0:
        reasons.append(f"Son 30 günde {muscle_tr} çalışmamışsın")
    elif max(freq.values(), default=1) > 0 and freq.get(target_muscle, 0) < max(freq.values()) * 0.3:
        reasons.append(f"{muscle_tr} egzersizlerini ihmal ediyorsun")

    goal = profile.get("goal", "GeneralFitness")
    goal_muscles = GOAL_MUSCLE_MAP.get(goal, GOAL_MUSCLE_MAP["GeneralFitness"])
    if target_muscle in goal_muscles["primary"]:
        goal_tr = _GOAL_TR.get(goal, goal)
        reasons.append(f"{goal_tr} hedefine uygun")

    if not reasons:
        reasons.append("Seviyene uygun, dengeli bir antrenman")

    return reasons
