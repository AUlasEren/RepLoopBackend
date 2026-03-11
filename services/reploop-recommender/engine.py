from __future__ import annotations

from feature_extractor import (
    difficulty_match_score,
    duration_match_score,
    volume_match_score,
    variety_score,
    muscle_group_variety_score,
    recency_boost,
)

# Agirliklar (6 sinyal, toplam = 1.0)
W_DIFFICULTY      = 0.25
W_DURATION        = 0.15
W_VOLUME          = 0.15
W_VARIETY         = 0.10
W_MUSCLE_VARIETY  = 0.25
W_RECENCY         = 0.10

# Sinyal isimleri ve Turkce aciklamalari (reason icin)
SIGNAL_LABELS = {
    "difficulty":      "Zorluk seviyene cok uygun",
    "duration":        "Antrenman suresi tam sana gore",
    "volume":          "Set ve tekrar hacmi seviyene uygun",
    "variety":         "Cesitlilik icin iyi bir tercih",
    "muscle_variety":  "Farkli kas gruplarini calistirmak icin ideal",
    "recency":         "Daha once basariyla tamamladin",
}

LEVEL_TAG_MAP = {
    "Beginner":     "Baslangic",
    "Intermediate": "Orta",
    "Advanced":     "Ileri",
}


def recommend(
    user: dict,
    workouts: list[dict],
    top_n: int = 5,
    session_history: list[dict] | None = None,
) -> list[dict]:
    """
    Weighted composite scoring ile kullaniciya en uygun top_n workout'i dondur.

    final_score = 0.25 * difficulty_match
                + 0.15 * duration_match
                + 0.15 * volume_match
                + 0.10 * variety_bonus
                + 0.25 * muscle_variety
                + 0.10 * recency_boost
    """
    if not workouts:
        return []

    history = session_history or []
    level = user.get("experience_level", "Intermediate")

    scored: list[tuple[dict, float, dict]] = []

    for w in workouts:
        wid = str(w["id"])

        signals = {
            "difficulty":     difficulty_match_score(w, level),
            "duration":       duration_match_score(w, level),
            "volume":         volume_match_score(w, level),
            "variety":        variety_score(wid, history),
            "muscle_variety": muscle_group_variety_score(w, history, workouts),
            "recency":        recency_boost(wid, history),
        }

        final = (
            W_DIFFICULTY     * signals["difficulty"]
            + W_DURATION     * signals["duration"]
            + W_VOLUME       * signals["volume"]
            + W_VARIETY      * signals["variety"]
            + W_MUSCLE_VARIETY * signals["muscle_variety"]
            + W_RECENCY      * signals["recency"]
        )

        scored.append((w, final, signals))

    # Skora gore azalan sirada sirala
    scored.sort(key=lambda x: x[1], reverse=True)

    results = []
    for w, final, signals in scored[:top_n]:
        results.append({
            **w,
            "score":  round(final, 3),
            "reason": _build_reason(signals),
            "tags":   _build_tags(w),
        })

    return results


def _build_reason(signals: dict) -> str:
    """En yuksek sinyale gore dinamik aciklama."""
    best_signal = max(signals, key=signals.get)
    return SIGNAL_LABELS.get(best_signal, "Seviyene uygun antrenman")


def _build_tags(workout: dict) -> list[str]:
    """Gercek kas grubu + sure + hareket sayisi."""
    tags = []

    # Seviye etiketi: cogunluk difficulty'den
    difficulties = workout.get("difficulties", [])
    if difficulties:
        adv = difficulties.count("Advanced")
        beg = difficulties.count("Beginner")
        total = len(difficulties)
        if adv / total > 0.5:
            tags.append(LEVEL_TAG_MAP["Advanced"])
        elif beg / total > 0.5:
            tags.append(LEVEL_TAG_MAP["Beginner"])
        else:
            tags.append(LEVEL_TAG_MAP["Intermediate"])

    # Kas gruplari (ilk 3)
    muscle_groups = workout.get("muscle_groups", [])
    for mg in muscle_groups[:3]:
        tags.append(mg)

    # Sure
    duration = workout.get("duration_minutes", 0)
    if duration:
        tags.append(f"{duration} dk")

    # Hareket sayisi
    exercise_count = workout.get("exercise_count", 0)
    if exercise_count:
        tags.append(f"{exercise_count} hareket")

    return tags
