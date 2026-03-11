"""
Weighted Composite Scoring icin 6 bagimsiz sinyal fonksiyonu.

Not: UserContext'teki age, weight_kg, height_cm alanlari kasitli olarak
kullanilmiyor. Bu degerler oneri kalitesini anlamli olcude etkilemiyor
ve gereksiz karmasiklik ekliyor. Ileride veri toplanirsa yeniden
degerlendirilebilir.
"""

from datetime import datetime, timezone

# Seviyeye gore beklenen zorluk dagilimi (Beginner/Intermediate/Advanced orani)
LEVEL_DIFFICULTY_PREFS = {
    "Beginner":     {"Beginner": 0.7, "Intermediate": 0.25, "Advanced": 0.05},
    "Intermediate": {"Beginner": 0.15, "Intermediate": 0.6, "Advanced": 0.25},
    "Advanced":     {"Beginner": 0.05, "Intermediate": 0.3, "Advanced": 0.65},
}

# Seviyeye gore ideal antrenman suresi araligi (dk)
LEVEL_DURATION_RANGE = {
    "Beginner":     (20, 40),
    "Intermediate": (35, 60),
    "Advanced":     (45, 90),
}

# Seviyeye gore ideal toplam set araligi
LEVEL_VOLUME_RANGE = {
    "Beginner":     (8, 16),
    "Intermediate": (14, 24),
    "Advanced":     (20, 35),
}


def difficulty_match_score(workout: dict, experience_level: str) -> float:
    """
    Workout'taki egzersiz zorluk dagilimi ile kullanici seviyesi uyumu.
    0.0 (tamamen uyumsuz) - 1.0 (mukemmel uyum)
    """
    difficulties = workout.get("difficulties", [])
    if not difficulties:
        return 0.5  # bilgi yok, notral

    total = len(difficulties)
    prefs = LEVEL_DIFFICULTY_PREFS.get(experience_level, LEVEL_DIFFICULTY_PREFS["Intermediate"])

    actual_dist = {}
    for d in difficulties:
        actual_dist[d] = actual_dist.get(d, 0) + 1

    # Her zorluk seviyesinin oranini tercihle karsilastir
    score = 0.0
    for level, pref_ratio in prefs.items():
        actual_ratio = actual_dist.get(level, 0) / total
        # 1 - |fark| : fark ne kadar kucukse skor o kadar yuksek
        score += (1.0 - abs(pref_ratio - actual_ratio)) * pref_ratio

    return min(max(score, 0.0), 1.0)


def duration_match_score(workout: dict, experience_level: str) -> float:
    """
    Workout suresi ile seviyeye gore ideal sure araligi uyumu.
    Aralik icindeyse 1.0, uzaklastikca azalir.
    """
    duration = workout.get("duration_minutes", 0)
    if duration <= 0:
        return 0.3  # bilgi yok

    lo, hi = LEVEL_DURATION_RANGE.get(experience_level, (30, 60))

    if lo <= duration <= hi:
        return 1.0

    # Aralik disindaysa mesafeye gore azalt
    if duration < lo:
        diff = lo - duration
    else:
        diff = duration - hi

    # Her 15 dk uzaklasma icin ~0.25 dusus
    penalty = diff / 60.0
    return max(1.0 - penalty, 0.0)


def volume_match_score(workout: dict, experience_level: str) -> float:
    """
    Toplam set hacmi ile seviyeye uygun hacim araligi uyumu.
    """
    total_sets = workout.get("total_sets", 0)
    if total_sets <= 0:
        return 0.5  # bilgi yok

    lo, hi = LEVEL_VOLUME_RANGE.get(experience_level, (12, 24))

    if lo <= total_sets <= hi:
        return 1.0

    if total_sets < lo:
        diff = lo - total_sets
    else:
        diff = total_sets - hi

    # Her 5 set uzaklasma icin ~0.25 dusus
    penalty = diff / 20.0
    return max(1.0 - penalty, 0.0)


def variety_score(workout_id: str, session_history: list[dict]) -> float:
    """
    Yakin zamanda yapilmamis workout'lara bonus.
    Hic yapilmamissa 1.0, son 7 gunde yapildiysa 0.2, arada kademeli.
    """
    if not session_history:
        return 1.0

    now = datetime.now(timezone.utc)

    # Bu workout'un en son yapildigi tarihi bul
    last_done = None
    for s in session_history:
        if s["workout_id"] == workout_id:
            completed = s["completed_at"]
            if completed is not None:
                # timezone-aware yap
                if completed.tzinfo is None:
                    completed = completed.replace(tzinfo=timezone.utc)
                if last_done is None or completed > last_done:
                    last_done = completed

    if last_done is None:
        return 1.0  # hic yapilmamis, maximum cesitlilik

    days_ago = (now - last_done).total_seconds() / 86400.0

    if days_ago <= 2:
        return 0.2
    elif days_ago <= 7:
        # 2-7 gun arasi: 0.2 -> 0.7 linear
        return 0.2 + (days_ago - 2) / 5.0 * 0.5
    elif days_ago <= 30:
        # 7-30 gun arasi: 0.7 -> 0.95 linear
        return 0.7 + (days_ago - 7) / 23.0 * 0.25
    else:
        return 1.0


def muscle_group_variety_score(
    workout: dict,
    session_history: list[dict],
    all_workouts: list[dict],
) -> float:
    """
    Son 4 gundeki session'larda calisan kas gruplariyla cakisma penaltisi.
    Cakisma yok → 1.0, yuksek cakisma → min 0.1.

    Decay: 0-2 gun → 1.0, 2-4 gun → 0.5, 4+ → ignore.
    Ek DB sorgusu yok — all_workouts listesinden workout_id → muscle_groups map olusturulur.
    """
    if not session_history:
        return 1.0

    candidate_groups = set(workout.get("muscle_groups", []))
    if not candidate_groups:
        return 0.5  # bilgi yok, notral

    now = datetime.now(timezone.utc)

    # workout_id -> muscle_groups map
    wid_to_groups: dict[str, set[str]] = {}
    for w in all_workouts:
        wid = str(w.get("id", ""))
        wid_to_groups[wid] = set(w.get("muscle_groups", []))

    # Son 4 gundeki session'lardan calisan kas gruplarini decay ile topla
    recent_group_weights: dict[str, float] = {}
    for s in session_history:
        completed = s.get("completed_at")
        if completed is None:
            continue
        if completed.tzinfo is None:
            completed = completed.replace(tzinfo=timezone.utc)

        days_ago = (now - completed).total_seconds() / 86400.0
        if days_ago > 4:
            continue

        decay = 1.0 if days_ago <= 2 else 0.5

        session_wid = str(s["workout_id"])
        groups = wid_to_groups.get(session_wid, set())
        for g in groups:
            recent_group_weights[g] = max(recent_group_weights.get(g, 0.0), decay)

    if not recent_group_weights:
        return 1.0  # yakin zamanda hicbir sey yapilmamis

    # Cakisma orani: aday kas gruplarinin ne kadari yakinda calisilmis
    overlap_sum = sum(recent_group_weights.get(g, 0.0) for g in candidate_groups)
    max_possible = len(candidate_groups)  # hepsi decay=1.0 olsa
    overlap_ratio = overlap_sum / max_possible

    # overlap_ratio 0 → 1.0, overlap_ratio 1 → 0.1
    score = 1.0 - 0.9 * overlap_ratio
    return max(score, 0.1)


def recency_boost(workout_id: str, session_history: list[dict]) -> float:
    """
    Daha once tamamlanmis (begenilmis) workout'lara bonus.
    Hic yapilmamissa 0.3, yapilmissa tamamlama sayisina gore artar.
    """
    if not session_history:
        return 0.3

    completions = sum(1 for s in session_history if s["workout_id"] == workout_id)

    if completions == 0:
        return 0.3
    elif completions == 1:
        return 0.6
    elif completions == 2:
        return 0.8
    else:
        return 1.0
