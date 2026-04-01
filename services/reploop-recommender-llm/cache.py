"""Workout ve session verileri icin TTL cache."""
from __future__ import annotations

import logging
import threading
import time

import database

logger = logging.getLogger(__name__)

_WORKOUT_TTL = 300          # 5 dakika
_SESSION_TTL = 60           # 1 dakika
_PUBLIC_EXERCISE_TTL = 1800 # 30 dakika
_USER_PROFILE_TTL = 300     # 5 dakika
_MUSCLE_FREQ_TTL = 60       # 1 dakika
_WORKOUT_EX_IDS_TTL = 60    # 1 dakika
_LLM_TEMPLATE_TTL = 1800   # 30 dakika

_lock = threading.Lock()
_workout_cache: dict[str, tuple[list[dict], float]] = {}
_session_cache: dict[str, tuple[list[dict], float]] = {}
_public_exercise_cache: dict[str, tuple[list[dict], float]] = {}
_user_profile_cache: dict[str, tuple[dict, float]] = {}
_muscle_freq_cache: dict[str, tuple[tuple[dict, set], float]] = {}
_workout_ex_ids_cache: dict[str, tuple[set[str], float]] = {}
_llm_template_cache: dict[str, tuple[list[dict], float]] = {}
_llm_in_progress: set[str] = set()


def get_all_workouts(user_id: str, *, use_mock: bool = False) -> list[dict]:
    """Workout verisini TTL cache ile dondur (user bazli)."""
    key = "mock" if use_mock else f"real:{user_id}"

    with _lock:
        if key in _workout_cache:
            value, ts = _workout_cache[key]
            if time.time() - ts < _WORKOUT_TTL:
                logger.debug("Workout cache hit (key=%s)", key)
                return value

    data = database.get_all_workouts_mock() if use_mock else database.get_all_workouts(user_id)

    with _lock:
        _workout_cache[key] = (data, time.time())

    return data


def get_session_history(user_id: str, *, use_mock: bool = False) -> list[dict]:
    """Session history'yi TTL cache ile dondur (user bazli, 60s)."""
    key = f"{'mock' if use_mock else 'real'}:{user_id}"

    with _lock:
        if key in _session_cache:
            value, ts = _session_cache[key]
            if time.time() - ts < _SESSION_TTL:
                logger.debug("Session cache hit (key=%s)", key)
                return value

    data = (
        database.get_session_history_mock(user_id)
        if use_mock
        else database.get_session_history(user_id)
    )

    with _lock:
        _session_cache[key] = (data, time.time())

    return data


def get_public_exercises() -> list[dict]:
    """Public exercise verisini TTL cache ile dondur (global, 30dk)."""
    key = "public"

    with _lock:
        if key in _public_exercise_cache:
            value, ts = _public_exercise_cache[key]
            if time.time() - ts < _PUBLIC_EXERCISE_TTL:
                logger.debug("Public exercise cache hit")
                return value

    data = database.get_public_exercises()

    with _lock:
        _public_exercise_cache[key] = (data, time.time())

    return data


def get_user_workout_exercise_ids(user_id: str) -> set[str]:
    """Kullanicinin mevcut workout'larindaki exercise ID'leri (1dk TTL)."""
    key = user_id

    with _lock:
        if key in _workout_ex_ids_cache:
            value, ts = _workout_ex_ids_cache[key]
            if time.time() - ts < _WORKOUT_EX_IDS_TTL:
                logger.debug("Workout exercise IDs cache hit (key=%s)", key)
                return value

    data = database.get_user_workout_exercise_ids(user_id)

    with _lock:
        _workout_ex_ids_cache[key] = (data, time.time())

    return data


def get_user_profile(user_id: str) -> dict:
    """User profile verisini TTL cache ile dondur (user bazli, 5dk)."""
    key = user_id

    with _lock:
        if key in _user_profile_cache:
            value, ts = _user_profile_cache[key]
            if time.time() - ts < _USER_PROFILE_TTL:
                logger.debug("User profile cache hit (key=%s)", key)
                return value

    data = database.get_user_profile(user_id)

    with _lock:
        _user_profile_cache[key] = (data, time.time())

    return data


def get_muscle_frequency(user_id: str) -> tuple[dict, set]:
    """Muscle frequency verisini TTL cache ile dondur (user bazli, 1dk)."""
    key = user_id

    with _lock:
        if key in _muscle_freq_cache:
            value, ts = _muscle_freq_cache[key]
            if time.time() - ts < _MUSCLE_FREQ_TTL:
                logger.debug("Muscle freq cache hit (key=%s)", key)
                return value

    data = database.get_muscle_frequency(user_id)

    with _lock:
        _muscle_freq_cache[key] = (data, time.time())

    return data


def discover_cache_key(user_id: str, workout_exercise_ids: set[str]) -> str:
    """user_id + workout havuzu hash'i — program degisince cache miss olur."""
    return f"{user_id}:{hash(frozenset(workout_exercise_ids))}"


def get_llm_templates(cache_key: str) -> list[dict] | None:
    """Cached LLM template'leri dondur, yoksa veya stale ise None."""
    with _lock:
        if cache_key in _llm_template_cache:
            value, ts = _llm_template_cache[cache_key]
            if time.time() - ts < _LLM_TEMPLATE_TTL:
                logger.debug("LLM template cache hit (key=%s)", cache_key)
                return value
    return None


def set_llm_templates(cache_key: str, templates: list[dict]):
    """LLM uretimi tamamlaninca cache'e yaz."""
    with _lock:
        _llm_template_cache[cache_key] = (templates, time.time())


def is_llm_in_progress(cache_key: str) -> bool:
    """Bu key icin LLM zaten arka planda calisiyor mu?"""
    with _lock:
        return cache_key in _llm_in_progress


def mark_llm_in_progress(cache_key: str, in_progress: bool):
    """LLM calisma durumunu isle."""
    with _lock:
        if in_progress:
            _llm_in_progress.add(cache_key)
        else:
            _llm_in_progress.discard(cache_key)


def invalidate_workouts():
    with _lock:
        _workout_cache.clear()


def invalidate_sessions(user_id: str | None = None):
    with _lock:
        if user_id:
            keys = [k for k in _session_cache if k.endswith(f":{user_id}")]
            for k in keys:
                del _session_cache[k]
        else:
            _session_cache.clear()
