#!/usr/bin/env python3
"""
Smoke test for /api/recommendations/ and /api/recommendations/discover endpoints.

Hedef:
  - Recommend endpoint: 5 sample request, JSON schema valid, algorithm field
    "llm-<model>" formatinda dinamik, latency log.
  - Discover endpoint: 5 sample request, JSON schema valid, response'taki
    exercise_id'lerin ExerciseDB'de gercekten var oldugunun dogrulamasi.

Kullanim:
  python smoke_test_endpoints.py                       # mevcut deployment'a karsi
  python smoke_test_endpoints.py --base http://localhost:5181
  python smoke_test_endpoints.py --check-llm-cache     # discover round 2: 60s bekle, LLM cache hit'i bekle
  python smoke_test_endpoints.py --expected-model reploop-fitness

Cikis kodu: 0 = tum testler PASS, 1 = en az bir FAIL
"""
from __future__ import annotations

import argparse
import json
import subprocess
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
from dataclasses import dataclass, field

# ---------------------------------------------------------------------------
# Test kullanicilari
# ---------------------------------------------------------------------------
# Real DB'den (UserDB/SessionDB/WorkoutDB) en zengin profil:
#   019cad69-eed5-7a47-ba31-10924bc2ee57 — 53 session, 1 workout
# Diger real user'lar workout/session bakimindan zayif → bos response normal.

USERS = [
    {
        "label": "real-rich",
        "user_id": "019cad69-eed5-7a47-ba31-10924bc2ee57",
        "age": 28, "weight_kg": 75.0, "height_cm": 178.0,
        "experience_level": "Intermediate", "goal": "MuscleGain",
    },
    {
        "label": "real-light",
        "user_id": "019cb8b0-71a3-7533-b1eb-7ead2b23941f",
        "age": 32, "weight_kg": 68.0, "height_cm": 170.0,
        "experience_level": "Beginner", "goal": "WeightLoss",
    },
    {
        "label": "real-workout-only",
        "user_id": "019c9f26-65e1-7799-8fa5-e46a2244314a",
        "age": 24, "weight_kg": 82.0, "height_cm": 185.0,
        "experience_level": "Advanced", "goal": "Endurance",
    },
    {
        "label": "synthetic-flex",
        "user_id": "00000000-0000-0000-0000-000000000004",
        "age": 35, "weight_kg": 60.0, "height_cm": 165.0,
        "experience_level": "Beginner", "goal": "Flexibility",
    },
    {
        "label": "synthetic-general",
        "user_id": "00000000-0000-0000-0000-000000000005",
        "age": 45, "weight_kg": 88.0, "height_cm": 175.0,
        "experience_level": "Intermediate", "goal": "GeneralFitness",
    },
]


_GOALS = ["MuscleGain", "WeightLoss", "Endurance", "Flexibility", "GeneralFitness"]
_LEVELS = ["Beginner", "Intermediate", "Advanced"]


def discover_users(n: int) -> list[dict]:
    """Discover stress icin n adet user; USERS'i tekrar et + synthetic UUID'lerle genislet."""
    out = list(USERS)
    while len(out) < n:
        i = len(out) + 1
        out.append({
            "label": f"synthetic-stress-{i:02d}",
            "user_id": f"00000000-0000-0000-0000-{i:012d}",
            "age": 25 + (i % 30),
            "weight_kg": 60.0 + (i % 40),
            "height_cm": 165.0 + (i % 25),
            "experience_level": _LEVELS[i % len(_LEVELS)],
            "goal": _GOALS[i % len(_GOALS)],
        })
    return out[:n]


# ---------------------------------------------------------------------------
# HTTP helpers (stdlib only)
# ---------------------------------------------------------------------------

@dataclass
class Result:
    ok: bool
    elapsed_ms: float
    status: int = 0
    data: dict = field(default_factory=dict)
    error: str = ""


def _request(method: str, url: str, body: dict | None, timeout: float) -> Result:
    t0 = time.time()
    try:
        if body is not None:
            data = json.dumps(body).encode("utf-8")
            req = urllib.request.Request(
                url, data=data, method=method,
                headers={"Content-Type": "application/json"},
            )
        else:
            req = urllib.request.Request(url, method=method)
        with urllib.request.urlopen(req, timeout=timeout) as r:
            payload = json.loads(r.read())
            return Result(ok=True, status=r.status, elapsed_ms=(time.time() - t0) * 1000, data=payload)
    except urllib.error.HTTPError as e:
        body_text = e.read().decode("utf-8", errors="replace")[:200]
        return Result(ok=False, status=e.code, elapsed_ms=(time.time() - t0) * 1000,
                      error=f"HTTP {e.code}: {body_text}")
    except Exception as e:
        return Result(ok=False, elapsed_ms=(time.time() - t0) * 1000,
                      error=f"{type(e).__name__}: {e}")


def post(url: str, body: dict, timeout: float) -> Result:
    return _request("POST", url, body, timeout)


def get(url: str, timeout: float) -> Result:
    return _request("GET", url, None, timeout)


# ---------------------------------------------------------------------------
# ExerciseDB UUID lookup (icin docker exec psql)
# ---------------------------------------------------------------------------

def fetch_exercise_db_ids() -> set[str] | None:
    """ExerciseDB'den public exercise UUID'lerini cek. Erisim yoksa None."""
    try:
        r = subprocess.run(
            [
                "docker", "exec", "-e", "PGPASSWORD=reploop123",
                "reploop-exercise-db",
                "psql", "-U", "reploop", "-d", "ExerciseDB", "-tAc",
                'SELECT "Id"::text FROM "Exercises" WHERE "IsPublic"=true;',
            ],
            capture_output=True, text=True, timeout=10,
        )
        if r.returncode != 0:
            print(f"  [WARN] ExerciseDB lookup failed: {r.stderr.strip()[:120]}")
            return None
        ids = {line.strip() for line in r.stdout.split("\n") if line.strip()}
        return ids
    except Exception as e:
        print(f"  [WARN] ExerciseDB lookup exception: {e}")
        return None


# ---------------------------------------------------------------------------
# Validators
# ---------------------------------------------------------------------------

def validate_recommend(data: dict, expected_algorithm: str) -> list[str]:
    errors: list[str] = []
    for key in ("user_id", "algorithm", "recommendations"):
        if key not in data:
            errors.append(f"missing key '{key}'")
    actual = data.get("algorithm", "")
    if actual != expected_algorithm:
        errors.append(f"algorithm mismatch: expected='{expected_algorithm}' actual='{actual}'")
    recs = data.get("recommendations", [])
    if not isinstance(recs, list):
        errors.append(f"'recommendations' is {type(recs).__name__}, expected list")
        return errors
    for i, r in enumerate(recs):
        for key in ("workout_id", "workout_name", "score", "reason", "tags"):
            if key not in r:
                errors.append(f"recommendations[{i}] missing key '{key}'")
    return errors


def validate_discover(data: dict, exercise_db_ids: set[str] | None) -> list[str]:
    errors: list[str] = []
    if "templates" not in data:
        errors.append("missing key 'templates'")
        return errors
    templates = data["templates"]
    if not isinstance(templates, list):
        errors.append(f"'templates' is {type(templates).__name__}, expected list")
        return errors
    required_t = ("name", "description", "duration_minutes", "difficulty",
                  "target_muscles", "exercises", "score", "score_reasons", "generated_by")
    required_ex = ("exercise_id", "name", "muscle_group", "equipment",
                   "difficulty", "sets", "reps")
    for i, t in enumerate(templates):
        for key in required_t:
            if key not in t:
                errors.append(f"templates[{i}] missing key '{key}'")
        for j, ex in enumerate(t.get("exercises", [])):
            for key in required_ex:
                if key not in ex:
                    errors.append(f"templates[{i}].exercises[{j}] missing '{key}'")
            ex_id = ex.get("exercise_id")
            if exercise_db_ids and ex_id and ex_id not in exercise_db_ids:
                errors.append(
                    f"templates[{i}].exercises[{j}].exercise_id={ex_id} NOT in ExerciseDB"
                )
    return errors


# ---------------------------------------------------------------------------
# Test runners
# ---------------------------------------------------------------------------

def run_recommend(base: str, expected_algorithm: str, timeout: float) -> tuple[int, int]:
    print(f"\n=== POST /api/recommendations/ ({len(USERS)} requests, timeout={timeout}s) ===")
    print(f"    expected algorithm field: '{expected_algorithm}'")
    print()
    pass_count = 0
    for u in USERS:
        body = {k: u[k] for k in ("user_id", "age", "weight_kg", "height_cm",
                                  "experience_level", "goal")}
        r = post(f"{base}/api/recommendations/", body=body, timeout=timeout)
        if not r.ok:
            print(f"  FAIL [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  {r.error}")
            continue
        errors = validate_recommend(r.data, expected_algorithm)
        rec_count = len(r.data.get("recommendations", []))
        algo = r.data.get("algorithm", "?")
        if not errors:
            pass_count += 1
            print(f"  PASS [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  algorithm={algo}  recs={rec_count}")
        else:
            print(f"  FAIL [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  algorithm={algo}  recs={rec_count}")
            for err in errors:
                print(f"         - {err}")
    return pass_count, len(USERS)


def run_discover(base: str, users: list[dict], exercise_db_ids: set[str] | None,
                 timeout: float, round_label: str = "round 1") -> tuple[int, int, list[str]]:
    print(f"\n=== GET /api/recommendations/discover ({round_label}, {len(users)} requests) ===")
    if exercise_db_ids is not None:
        print(f"    ExerciseDB UUID dogrulama aktif ({len(exercise_db_ids)} ID yuklendi)")
    else:
        print(f"    [WARN] ExerciseDB UUID dogrulama atlandi")
    print()
    pass_count = 0
    generated_by_summary: list[str] = []
    for u in users:
        url = f"{base}/api/recommendations/discover?user_id={urllib.parse.quote(u['user_id'])}"
        r = get(url, timeout=timeout)
        if not r.ok:
            print(f"  FAIL [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  {r.error}")
            continue
        errors = validate_discover(r.data, exercise_db_ids)
        templates = r.data.get("templates", [])
        gb = templates[0].get("generated_by", "n/a") if templates else "empty"
        generated_by_summary.append(gb)
        if not errors:
            pass_count += 1
            print(f"  PASS [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  templates={len(templates)}  first.generated_by={gb}")
        else:
            print(f"  FAIL [{u['label']:20s}] {r.elapsed_ms:7.0f}ms  templates={len(templates)}  first.generated_by={gb}")
            for err in errors[:5]:  # first 5 only
                print(f"         - {err}")
            if len(errors) > 5:
                print(f"         ... {len(errors) - 5} more errors")
    return pass_count, len(users), generated_by_summary


def dump_discover_logs(since_seconds: int = 600) -> None:
    """Recommender container'indan son N saniyenin Discover LLM log'larini dump et."""
    print(f"\n=== docker logs reploop-recommender-service (Discover LLM/retry, last {since_seconds}s) ===")
    try:
        r = subprocess.run(
            ["docker", "logs", "--since", f"{since_seconds}s", "reploop-recommender-service"],
            capture_output=True, text=True, timeout=10,
        )
        lines = (r.stdout + r.stderr).split("\n")
        keywords = ("Discover LLM", "succeeded on attempt", "denemede basarisiz",
                    "LLM enrichment", "Returning cached LLM")
        filtered = [l for l in lines if any(k in l for k in keywords)]
        if not filtered:
            print("  (no matching log lines)")
            return
        for line in filtered[-50:]:  # last 50
            print(f"  {line}")
    except Exception as e:
        print(f"  [WARN] log dump failed: {e}")


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--base", default="http://localhost:5181",
                    help="Recommender service base URL (default: http://localhost:5181)")
    ap.add_argument("--expected-model", default="reploop-fitness",
                    help="Beklenen Ollama model adi (algorithm field check). Default: reploop-fitness")
    ap.add_argument("--recommend-timeout", type=float, default=120.0,
                    help="Recommend endpoint timeout (sn). Default: 120")
    ap.add_argument("--discover-timeout", type=float, default=30.0,
                    help="Discover endpoint timeout (sn). Default: 30 (algorithmic hizli)")
    ap.add_argument("--check-llm-cache", action="store_true",
                    help="Discover round 2: --cache-wait sn bekle, LLM cache hit ('generated_by'='llm') bekle")
    ap.add_argument("--discover-runs", type=int, default=10,
                    help="Discover endpoint'ine sequential request sayisi (default: 10, daemon stress test)")
    ap.add_argument("--cache-wait", type=int, default=120,
                    help="--check-llm-cache modu icin background LLM enrichment bekleme suresi (sn). Default: 120")
    args = ap.parse_args()

    base = args.base.rstrip("/")
    expected_algorithm = f"llm-{args.expected_model}"

    print(f"Smoke test target: {base}")
    print(f"Expected model:    {args.expected_model}")

    # Health check
    print("\n=== GET /api/recommendations/health ===")
    h = get(f"{base}/api/recommendations/health", timeout=5)
    if not h.ok:
        print(f"  FAIL  health {h.elapsed_ms:.0f}ms  {h.error}")
        print("  Recommender erisilemiyor; testi durdur.")
        return 1
    print(f"  PASS  health {h.elapsed_ms:.0f}ms  {h.data}")

    # ExerciseDB UUID lookup
    print("\n=== ExerciseDB UUID lookup ===")
    ex_ids = fetch_exercise_db_ids()
    if ex_ids is not None:
        print(f"  Loaded {len(ex_ids)} public exercise UUIDs from ExerciseDB")

    # Recommend
    rec_pass, rec_total = run_recommend(base, expected_algorithm, args.recommend_timeout)

    # Discover round 1 (stress test)
    discover_user_list = discover_users(args.discover_runs)
    dis1_pass, dis1_total, gb1 = run_discover(base, discover_user_list, ex_ids,
                                              args.discover_timeout, f"round 1, {args.discover_runs} stress")

    # Optional discover round 2
    dis2_pass, dis2_total, gb2 = 0, 0, []
    if args.check_llm_cache:
        print(f"\n=== Sleeping {args.cache_wait}s for background LLM enrichment ({args.discover_runs} parallel threads) ===")
        time.sleep(args.cache_wait)
        dis2_pass, dis2_total, gb2 = run_discover(base, discover_user_list, ex_ids,
                                                  args.discover_timeout, "round 2 (LLM cache)")

    # Server-side log dump (retry/attempt analizi icin)
    dump_discover_logs(since_seconds=600)

    # Summary
    print("\n" + "=" * 60)
    print("SUMMARY")
    print("=" * 60)
    print(f"  Recommend:        {rec_pass}/{rec_total} PASS")
    print(f"  Discover round 1: {dis1_pass}/{dis1_total} PASS  ({args.discover_runs} sequential)")
    print(f"                    generated_by={gb1}")
    if args.check_llm_cache:
        llm_count = sum(1 for g in gb2 if g == "llm")
        print(f"  Discover round 2: {dis2_pass}/{dis2_total} PASS  llm_count={llm_count}/{dis2_total}")
        print(f"                    generated_by={gb2}")
    print("=" * 60)

    overall = (rec_pass == rec_total) and (dis1_pass == dis1_total)
    if args.check_llm_cache:
        overall = overall and (dis2_pass == dis2_total)
        # User explicit: %100 LLM cevap bekleniyor (fallback yok). round 2 gb2'de hepsi "llm" olmali.
        llm_count = sum(1 for g in gb2 if g == "llm")
        if llm_count < len(gb2):
            print(f"\n  [WARN] Discover round 2: {len(gb2) - llm_count}/{len(gb2)} request fallback'e dustu")
            print(f"         %100 LLM cevap bekleniyordu — daemon stability sorunu olabilir.")
            overall = False
    return 0 if overall else 1


if __name__ == "__main__":
    sys.exit(main())
