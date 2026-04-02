"""
discover_llm.py — LLM katmani (Ollama/gemma2).

Pre-filtered 15 exercise + user profile alir,
3 workout template olusturur (isim, aciklama, exercise sirasi, sets/reps).
Basarisiz olursa None doner — router fallback'e duser.
"""
from __future__ import annotations

import json
import logging
from typing import Optional

import ollama

from engine2 import OLLAMA_MODEL

logger = logging.getLogger(__name__)

_DISCOVER_TIMEOUT = 180.0   # saniye — Docker CPU inference icin
_DISCOVER_MAX_TOKENS = 2048  # 3 template JSON icin yeterli

SYSTEM_PROMPT = """\
Sen bir kisisel antrenor asistanisin.
Sana bir kullanici profili ve skorlanmis egzersiz listesi verilecek.
Gorevin: Bu egzersizlerden 3 farkli workout template olusturmak.

KURALLAR:
- Her template 4-5 egzersiz icermeli.
- Compound hareketler once, isolation hareketler sonda olmali.
- Agonist-antagonist dengesini koru.
- Ayni egzersiz birden fazla template'te olmamali.
- Sets ve reps kullanicinin seviyesine uygun olmali (Beginner: 3x12, Intermediate: 4x10, Advanced: 5x6).
- Template isimleri yaratici ve motive edici olmali (Turkce).
- Aciklamalar kisisellestirilmis olmali (kullanicinin goal'une ve eksik kas gruplarina referans).

SADECE JSON formatinda yanit ver, baska hicbir sey yazma. Markdown backtick kullanma.
Asagidaki JSON schema'ya uy:

{
  "templates": [
    {
      "name": "string (yaratici Turkce isim)",
      "description": "string (kisisellestirilmis aciklama)",
      "duration_minutes": int,
      "difficulty": "string",
      "target_muscles": ["string"],
      "exercises": [
        {
          "exercise_id": "string (verilen ID'lerden biri)",
          "name": "string",
          "muscle_group": "string",
          "equipment": "string",
          "difficulty": "string",
          "sets": int,
          "reps": int
        }
      ],
      "score_reasons": ["string (neden bu template oneriliyor, Turkce)"]
    }
  ]
}"""


def _build_user_prompt(candidates: list[dict], user_profile: dict) -> str:
    """LLM'e gonderilecek kullanici mesajini olustur."""
    exercises_text = json.dumps(
        [
            {
                "id": e["Id"],
                "name": e["Name"],
                "muscle_group": e.get("MuscleGroup", ""),
                "equipment": e.get("Equipment", ""),
                "difficulty": e.get("Difficulty", ""),
                "score": e.get("_score", 0),
            }
            for e in candidates
        ],
        ensure_ascii=False,
        indent=2,
    )

    freq_text = json.dumps(user_profile.get("muscle_frequency", {}), ensure_ascii=False)
    equip_text = ", ".join(user_profile.get("equipment_used", set())) or "Bilgi yok"

    return (
        f"Kullanici Profili:\n"
        f"- Seviye: {user_profile.get('experience_level', 'Beginner')}\n"
        f"- Hedef: {user_profile.get('goal', 'GeneralFitness')}\n"
        f"- Son 30 gun kas grubu frekansi: {freq_text}\n"
        f"- Kullandigi ekipmanlar: {equip_text}\n\n"
        f"Skorlanmis Egzersiz Havuzu ({len(candidates)} adet):\n"
        f"{exercises_text}\n\n"
        f"Bu egzersizlerden 3 workout template olustur."
    )


def generate_templates_with_llm(
    candidates: list[dict],
    user_profile: dict,
) -> Optional[list[dict]]:
    """
    Ollama/gemma2 ile template uret.
    Basarisiz olursa None doner (router fallback'e duser).
    """
    valid_ids = {e["Id"] for e in candidates}
    candidate_map = {e["Id"]: e for e in candidates}
    user_prompt = _build_user_prompt(candidates, user_profile)

    try:
        client = ollama.Client(timeout=_DISCOVER_TIMEOUT)
        response = client.chat(
            model=OLLAMA_MODEL,
            messages=[
                {"role": "system", "content": SYSTEM_PROMPT},
                {"role": "user", "content": user_prompt},
            ],
            format="json",
            options={"num_predict": _DISCOVER_MAX_TOKENS},
        )
        raw_text = response["message"]["content"]
    except Exception as e:
        logger.warning("Discover LLM call failed: %s", e)
        return None

    # JSON parse
    try:
        parsed = json.loads(raw_text)
    except json.JSONDecodeError:
        logger.warning("Discover LLM JSON parse failed. Raw length: %d, last 200 chars: ...%s", len(raw_text), raw_text[-200:])
        return None

    # templates listesini cikar
    templates = _extract_templates(parsed)
    if not templates:
        logger.warning("Discover LLM returned no valid templates")
        return None

    # Validate & enrich
    result: list[dict] = []
    for t in templates:
        exercises = t.get("exercises", [])
        # Sadece valid ID'li exercise'leri kabul et
        valid_exercises = [e for e in exercises if e.get("exercise_id") in valid_ids]
        if not valid_exercises:
            continue

        # Score: template'teki exercise'lerin algoritmik skorlarinin ortalamasi
        scores = [candidate_map[e["exercise_id"]]["_score"] for e in valid_exercises if e["exercise_id"] in candidate_map]
        avg_score = round(sum(scores) / max(len(scores), 1), 2)

        result.append({
            "name": t.get("name", "Workout"),
            "description": t.get("description", ""),
            "duration_minutes": t.get("duration_minutes", len(valid_exercises) * 8 + 5),
            "difficulty": t.get("difficulty", user_profile.get("experience_level", "Beginner")),
            "target_muscles": t.get("target_muscles", []),
            "exercises": valid_exercises,
            "score": avg_score,
            "score_reasons": t.get("score_reasons", []),
            "generated_by": "llm",
        })

    return result if result else None


def _extract_templates(parsed) -> list[dict]:
    """JSON parse sonucundan templates listesini cikar."""
    if isinstance(parsed, dict):
        if "templates" in parsed and isinstance(parsed["templates"], list):
            return parsed["templates"]
        for value in parsed.values():
            if isinstance(value, list):
                return value
    if isinstance(parsed, list):
        return parsed
    return []
