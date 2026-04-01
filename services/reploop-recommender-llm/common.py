"""Recommendation engine'ler arasi paylasilan yardimci fonksiyonlar."""

LEVEL_TAG_MAP = {
    "Beginner": "Baslangic",
    "Intermediate": "Orta",
    "Advanced": "Ileri",
}


def build_tags(workout: dict) -> list[str]:
    """Workout metadata'sindan etiket listesi olustur."""
    tags: list[str] = []

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

    for mg in workout.get("muscle_groups", [])[:3]:
        tags.append(mg)

    duration = workout.get("duration_minutes", 0)
    if duration:
        tags.append(f"{duration} dk")

    exercise_count = workout.get("exercise_count", 0)
    if exercise_count:
        tags.append(f"{exercise_count} hareket")

    return tags
