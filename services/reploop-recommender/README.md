# RepLoop Recommender Service

İçerik tabanlı (content-based) antrenman öneri motoru. FastAPI üzerinde çalışır, kullanıcının profili ve geçmiş antrenman oturumları ışığında **weighted composite scoring** (v3) ile workout kütüphanesinden en uygunları sıralar.

- **Stack:** Python 3.12, FastAPI, psycopg2
- **Algoritma:** `composite-scoring-v3` — 6 sinyalin ağırlıklı toplamı
- **Port:** 8000 (Docker) / 5181 (local dev)
- **Gateway path:** `/api/recommendations/**`

---

## Endpoint'ler

### `POST /api/recommendations/`

Kullanıcı bağlamına göre senkron olarak öneri üretir.

**Query param:** `top_n` (varsayılan 5, min 1, max 20)

**Request body (`UserContext`):**
```json
{
  "user_id": "uuid-string",
  "age": 27,
  "weight_kg": 78.5,
  "height_cm": 180,
  "experience_level": "Intermediate",
  "goal": "MuscleGain"
}
```

> Not: `age`, `weight_kg`, `height_cm` ve `goal` şu an skorlamaya girmiyor (bkz. [Bilinen Sınırlamalar](#bilinen-s%C4%B1n%C4%B1rlamalar)). `experience_level` ana sürücü.

**Response:**
```json
{
  "user_id": "...",
  "algorithm": "composite-scoring-v3",
  "recommendations": [
    {
      "workout_id": "...",
      "workout_name": "İtme Günü",
      "description": "Göğüs & Omuz & Triceps",
      "duration_minutes": 60,
      "exercise_count": 6,
      "muscle_groups": ["Chest", "Shoulders", "Triceps"],
      "score": 0.812,
      "reason": "Zorluk seviyene çok uygun",
      "tags": ["Orta", "Chest", "Shoulders", "Triceps", "60 dk", "6 hareket"]
    }
  ]
}
```

**Hata:** DB'ye ulaşılamazsa `503 Database unavailable`.

### `GET /api/recommendations/health`

```json
{ "status": "ok", "service": "reploop-recommender" }
```

---

## Skorlama Modeli

Her workout için 6 bağımsız sinyal `[0, 1]` aralığında hesaplanır ve ağırlıklı toplamı alınır:

```
final = 0.25 · difficulty
      + 0.25 · muscle_variety
      + 0.15 · duration
      + 0.15 · volume
      + 0.10 · variety
      + 0.10 · recency
```

| Sinyal | Ağırlık | Ne ölçer |
|---|---|---|
| `difficulty` | 0.25 | Workout'taki egzersizlerin zorluk dağılımı, kullanıcının seviye tercih profiline ne kadar yakın |
| `muscle_variety` | 0.25 | Son 4 gündeki oturumlarda çalışılan kas gruplarıyla örtüşme (cakışma → ceza) |
| `duration` | 0.15 | Süre, seviyeye özel ideal aralığın içinde mi (örn. Intermediate 35–60 dk) |
| `volume` | 0.15 | Toplam set sayısı, seviyeye özel hacim aralığında mı (örn. Intermediate 14–24 set) |
| `variety` | 0.10 | Aynı workout yakın zamanda yapılmış mı (2 gün → 0.2, 30+ gün → 1.0) |
| `recency` | 0.10 | Kullanıcının daha önce bu workout'u kaç kez tamamladığı (0 → 0.3, 3+ → 1.0) |

Sıralama sonrası ilk `top_n` öneri döner. `reason` alanı en yüksek sinyale göre dinamik üretilir; `tags` ise workout'un baskın zorluğu + ilk 3 kas grubu + süre + hareket sayısıdır.

### Seviye parametreleri (`feature_extractor.py`)

```
LEVEL_DIFFICULTY_PREFS   # Beginner/Intermediate/Advanced için beklenen egzersiz dağılımı
LEVEL_DURATION_RANGE     # Beginner 20-40, Intermediate 35-60, Advanced 45-90 dk
LEVEL_VOLUME_RANGE       # Beginner 8-16, Intermediate 14-24, Advanced 20-35 set
```

---

## Veri Kaynakları

Şu an servis, diğer mikroservislerin PostgreSQL veritabanlarını **doğrudan** okuyor (read-only):

| DB | Port | Ne alınıyor |
|---|---|---|
| `WorkoutDB` | 5433 | `Workouts`, `WorkoutExercises` (sets, reps, weight) |
| `ExerciseDB` | 5434 | `Exercises` (MuscleGroup, Difficulty) — WorkoutExercise'lerle ExerciseId veya normalize isim üzerinden join |
| `SessionDB` | 5437 | `Sessions` — yalnızca `Status = 'Completed'` olanlar |

> **Not (mimari borç):** Mikroservis saflığı açısından ideal yaklaşım, veriyi ilgili servislerin HTTP API'leri üzerinden çekmektir. Doğrudan DB okuması geliştirme hızı için tercih edildi; production öncesi HTTP tabanlı okumaya geçiş planlanıyor.

**DB yokken test için** `USE_MOCK_DB=true` ortam değişkeni ile `database.py` içindeki mock workout/session listesi kullanılır.

---

## Proje Yapısı

```
reploop-recommender/
├── main.py                # FastAPI app kurulumu
├── router.py              # /api/recommendations/ endpoint'leri
├── schemas.py             # Pydantic modelleri (UserContext, RecommendationResponse)
├── engine.py              # Ağırlıklı toplam + reason/tag üretimi
├── feature_extractor.py   # 6 sinyal fonksiyonu + seviye parametreleri
├── database.py            # 3 DB'ye psycopg2 bağlantıları + mock veri
├── requirements.txt
├── Dockerfile
└── .env.example
```

---

## Çalıştırma

### Docker (önerilen)

Kök dizinden:
```bash
docker compose up -d reploop-recommender
```

Servis container içinde 8000 portunda ayağa kalkar; `docker-compose.yml` içinde 5181 olarak yayınlanır. DB host'ları compose network'ünde `reploop-workout-db`, `reploop-exercise-db`, `reploop-session-db` olarak çözülür.

### Local dev

```bash
cd services/reploop-recommender
python3 -m venv venv && source venv/bin/activate
pip install -r requirements.txt
cp .env.example .env       # değerleri aşağıdaki tabloya göre düzenle
uvicorn main:app --reload --port 5181
```

Swagger UI: <http://localhost:5181/docs>

### Hızlı test (DB olmadan)

```bash
USE_MOCK_DB=true uvicorn main:app --port 5181

curl -X POST "http://localhost:5181/api/recommendations/?top_n=3" \
  -H "Content-Type: application/json" \
  -d '{
    "user_id": "00000000-0000-0000-0000-000000000001",
    "age": 27, "weight_kg": 78, "height_cm": 180,
    "experience_level": "Intermediate", "goal": "MuscleGain"
  }'
```

---

## Ortam Değişkenleri

| Değişken | Varsayılan | Açıklama |
|---|---|---|
| `WORKOUT_DB_HOST` | `localhost` | WorkoutDB host'u |
| `WORKOUT_DB_PORT` | `5433` | |
| `WORKOUT_DB_NAME` | `WorkoutDB` | |
| `EXERCISE_DB_HOST` | `localhost` | ExerciseDB host'u |
| `EXERCISE_DB_PORT` | `5434` | |
| `EXERCISE_DB_NAME` | `ExerciseDB` | |
| `SESSION_DB_HOST` | `localhost` | SessionDB host'u |
| `SESSION_DB_PORT` | `5437` | |
| `SESSION_DB_NAME` | `SessionDB` | |
| `DB_USER` | `reploop` | Üç DB için ortak kullanıcı |
| `DB_PASSWORD` | `reploop123` | |
| `USE_MOCK_DB` | `false` | `true` yapılırsa DB'ye bağlanmaz, sabit mock veri döner |

---

## Bilinen Sınırlamalar

Sunum/tartışma sırasında şunları bilerek iletmek gerekir:

- **Bağlantı havuzu yok.** Her istek 3 ayrı `psycopg2.connect()` açıp kapatıyor. Yük altında connection churn olabilir; pool (örn. `psycopg2.pool.ThreadedConnectionPool`) eklenmesi kolay bir iyileştirme.
- **Önbellek yok.** Her istekte tüm workout kütüphanesi ve kullanıcının tüm completed session'ları yeniden çekilir. Planlanan: önce süreç-içi TTL cache, sonrasında Redis.
- **Zamanlayıcı yok.** Öneriler önceden hesaplanıp saklanmıyor; her istekte sıfırdan üretiliyor. Gece yarısı toplu refresh gibi bir pipeline henüz yok.
- **Profil HTTP'den değil, request body'den geliyor.** Servis UserDB'ye bağlanmıyor; caller (şu an gateway üzerinden client) profil alanlarını kendi dolduruyor.
- **Skora girmeyen alanlar:** `goal`, `equipment` (modellenmedi), `age`, `weight_kg`, `height_cm`. `feature_extractor.py` başındaki nota göre bu alanlar öneri kalitesine anlamlı katkı vermediği için kasıtlı olarak dışarıda bırakıldı.
- **Session tarih filtresi SQL'de değil kodda.** `get_session_history` tüm completed oturumları çekiyor; 2–4 gün / 30 gün pencereleri sinyal fonksiyonları içinde uygulanıyor. Geçmişi çok birikmiş kullanıcılar için ileride SQL'de `WHERE CompletedAt > NOW() - INTERVAL '30 days'` eklenmesi makul.
- **HTTP geçişine hazırlık arayüzleri henüz yok.** `IUserProfileReader` / `ISessionHistoryReader` gibi soyutlamalar planda; şu an `database.py` içinde raw SQL.

---

## Skor Algoritmasının Evrimi

- **v1:** Basit toplam (tüm boyutlar eşit ağırlıkta). Bir boyutta güçlü ama kas grubu uyumsuz workout listeye çıkabiliyordu.
- **v2:** Çarpımsal model. Tek boyutta sıfır alan workout tamamen eleniyordu; katalogun büyük kısmı filtreleniyordu.
- **v3 (mevcut):** Ağırlıklı toplam + `muscle_variety` (cakışma cezası) + `variety` (tekrar cezası) + `recency` (geçmişte beğenilmişe bonus). Trade-off: `variety` tekrarı caydırırken `recency` tekrarı ödüllendirir; ikisi kasten zıt yönde çalışıp dengeleme yapar.