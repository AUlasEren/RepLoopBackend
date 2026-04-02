"""
engine2.py — LLM-destekli antrenman oneri motoru.

engine.py ile ayni fonksiyon imzasi:
    recommend(user, workouts, top_n, session_history) -> list[dict]

Ollama API uzerinden lokal LLM modeli kullanilir (varsayilan: gemma2:9b).
JSON mode ile yapisal cikti garanti edilir.
Basarisiz olursa engine.py'nin algorithmic scoring'ine fallback yapar.
"""

from __future__ import annotations

import json
import logging
import os
import sys
from datetime import datetime, timezone

import ollama

from common import build_tags

logger = logging.getLogger(__name__)

# Konfigürasyon (env ile override edilebilir)
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "gemma2:9b")
LLM_MAX_RETRIES = int(os.getenv("LLM_MAX_RETRIES", "2"))
LLM_MAX_TOKENS = int(os.getenv("LLM_MAX_TOKENS", "1024"))
LLM_STREAM = os.getenv("LLM_STREAM", "false").lower() == "true"

SYSTEM_PROMPT = """\
Sen bir kisisel antrenman oneri asistanisin.
Kullanici bilgilerini ve mevcut antrenman listesini inceleyerek en uygun antrenmanlari secersin.

Kurallar:
- Yalnizca verilen workout ID'lerini kullan, yeni ID uretme.
- Yakin zamanda yapilmis antrenmanlardan kacinmaya calis.
- Farkli kas gruplari calistiran cesitli onerilerde bulun.
- Her oneri icin kisa ve anlamli Turkce aciklama yaz (max 10 kelime).
- Yanit olarak SADECE JSON dondur.

JSON formati:
[
  {"id": "<workout id>", "reason": "<kisa Turkce aciklama>"},
  ...
]"""


# ---------------------------------------------------------------------------
# Public API
# ---------------------------------------------------------------------------

def recommend(
    user: dict,
    workouts: list[dict],
    top_n: int = 5,
    session_history: list[dict] | None = None,
) -> list[dict]:
    """
    Ollama LLM ile kullaniciya en uygun top_n workout'u dondur.
    Basarisiz olursa engine.py'nin algorithmic scoring'ine fallback yapar.
    """
    if not workouts:
        return []

    history = session_history or []
    prompt = _build_prompt(user, workouts, history, top_n)

    ranked_ids = _call_llm_with_retry(prompt, workouts)

    if not ranked_ids:
        logger.warning("LLM %d denemede basarisiz, algorithmic engine'e fallback", LLM_MAX_RETRIES)
        return _fallback_recommend(user, workouts, top_n, history)

    workout_map = {str(w["id"]): w for w in workouts}
    results: list[dict] = []

    for rank, (wid, reason) in enumerate(ranked_ids[:top_n], start=1):
        w = workout_map.get(wid)
        if w is None:
            continue
        score = round(1.0 - (rank - 1) / max(top_n, 1) * 0.4, 3)
        results.append({
            **w,
            "score": score,
            "reason": reason,
            "tags": build_tags(w),
        })

    return results


# ---------------------------------------------------------------------------
# LLM call with retry
# ---------------------------------------------------------------------------

def _call_llm_with_retry(prompt: str, workouts: list[dict]) -> list[tuple[str, str]]:
    """LLM'i retry ile cagir. Basarisizsa bos liste dondur."""
    for attempt in range(1, LLM_MAX_RETRIES + 1):
        try:
            raw_text = _call_ollama(prompt)
            ranked = _parse_ranking(raw_text, workouts)
            if ranked:
                return ranked
            logger.warning("LLM parse basarisiz (deneme %d/%d)", attempt, LLM_MAX_RETRIES)
        except Exception as e:
            logger.warning("LLM cagri hatasi (deneme %d/%d): %s", attempt, LLM_MAX_RETRIES, e)
    return []


def _call_ollama(prompt: str) -> str:
    """Ollama API'sini JSON mode ile cagir."""
    messages = [
        {"role": "system", "content": SYSTEM_PROMPT},
        {"role": "user", "content": prompt},
    ]

    if LLM_STREAM:
        return _stream_ollama(messages)

    response = ollama.chat(
        model=OLLAMA_MODEL,
        messages=messages,
        format="json",
        options={"num_predict": LLM_MAX_TOKENS},
    )
    return response["message"]["content"]


def _stream_ollama(messages: list[dict]) -> str:
    """Streaming modda cagir, token'lari terminal'e yazar."""
    collected: list[str] = []
    stream = ollama.chat(
        model=OLLAMA_MODEL,
        messages=messages,
        format="json",
        options={"num_predict": LLM_MAX_TOKENS},
        stream=True,
    )
    for chunk in stream:
        token = chunk["message"]["content"]
        sys.stdout.write(token)
        sys.stdout.flush()
        collected.append(token)
    print()
    return "".join(collected)


# ---------------------------------------------------------------------------
# Fallback: algorithmic engine
# ---------------------------------------------------------------------------

def _fallback_recommend(
    user: dict,
    workouts: list[dict],
    top_n: int,
    session_history: list[dict],
) -> list[dict]:
    """engine.py'nin weighted composite scoring'ini fallback olarak kullan."""
    import engine as algorithmic_engine
    return algorithmic_engine.recommend(
        user=user,
        workouts=workouts,
        top_n=top_n,
        session_history=session_history,
    )


# ---------------------------------------------------------------------------
# Prompt builder
# ---------------------------------------------------------------------------

def _build_prompt(
    user: dict,
    workouts: list[dict],
    history: list[dict],
    top_n: int,
) -> str:
    goal_tr = {
        "WeightLoss": "kilo vermek",
        "MuscleGain": "kas kazanmak",
        "Endurance": "dayaniklilik gelistirmek",
        "Flexibility": "esneklik kazanmak",
        "GeneralFitness": "genel kondisyon saglamak",
    }.get(user.get("goal", ""), user.get("goal", ""))

    level = user.get("experience_level", "Intermediate")
    recent_map = _recent_workout_map(history, days=7)

    workout_lines: list[str] = []
    for w in workouts:
        wid = str(w["id"])
        muscles = ", ".join(w.get("muscle_groups", [])) or "belirtilmemis"

        recency_note = ""
        if wid in recent_map:
            days_ago = recent_map[wid]
            if days_ago < 1:
                recency_note = " (bugun yapildi)"
            else:
                recency_note = f" ({int(days_ago)} gun once yapildi)"

        workout_lines.append(
            f"  - ID: {wid} | {w['name']} | {w.get('duration_minutes', 0)} dk "
            f"| {w.get('exercise_count', 0)} hareket | Kaslar: {muscles} "
            f"| Toplam set: {w.get('total_sets', 0)}{recency_note}"
        )

    workouts_text = "\n".join(workout_lines)

    return f"""\
KULLANICI:
  - Hedef: {goal_tr}
  - Seviye: {level}
  - Yas: {user.get("age", "?")} | Kilo: {user.get("weight_kg", "?")} kg | Boy: {user.get("height_cm", "?")} cm

ANTRENMANLAR:
{workouts_text}

Yukaridaki antrenmanlardan bu kullaniciya en uygun {top_n} tanesini sec.
Sadece JSON dondur."""


# ---------------------------------------------------------------------------
# Response parser
# ---------------------------------------------------------------------------

def _parse_ranking(raw_text: str, workouts: list[dict]) -> list[tuple[str, str]]:
    """
    LLM metninden JSON listesini ayikla.
    JSON mode sayesinde valid JSON beklenir ama savunmasal parse yapar.
    """
    valid_ids = {str(w["id"]) for w in workouts}

    try:
        parsed = json.loads(raw_text)
    except json.JSONDecodeError:
        # JSON mode'a ragmen parse hatasi — metin icinden JSON blok bul
        start = raw_text.find("[")
        end = raw_text.rfind("]") + 1
        if start == -1 or end == 0:
            return []
        try:
            parsed = json.loads(raw_text[start:end])
        except json.JSONDecodeError:
            return []

    # Dizi veya dict icinden dizi cikar
    items = _extract_list(parsed)
    if not items:
        return []

    result: list[tuple[str, str]] = []
    for item in items:
        if not isinstance(item, dict):
            continue
        wid = str(item.get("id", "")).strip()
        reason = str(item.get("reason", "Seviyene uygun antrenman")).strip()
        if wid in valid_ids:
            result.append((wid, reason))

    return result


def _extract_list(parsed) -> list | None:
    """JSON parse sonucundan listeyi cikar (dizi veya dict icindeki ilk dizi)."""
    if isinstance(parsed, list):
        return parsed
    if isinstance(parsed, dict):
        for value in parsed.values():
            if isinstance(value, list):
                return value
    return None


# ---------------------------------------------------------------------------
# Yardimci
# ---------------------------------------------------------------------------

def _recent_workout_map(history: list[dict], days: int = 7) -> dict[str, float]:
    """Son N gunde yapilmis workout'larin ID -> gun once map'i."""
    now = datetime.now(timezone.utc)
    recent: dict[str, float] = {}
    for s in history:
        completed = s.get("completed_at")
        if completed is None:
            continue
        if completed.tzinfo is None:
            completed = completed.replace(tzinfo=timezone.utc)
        days_ago = (now - completed).total_seconds() / 86400.0
        if days_ago <= days:
            wid = str(s["workout_id"])
            if wid not in recent or days_ago < recent[wid]:
                recent[wid] = days_ago
    return recent
