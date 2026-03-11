# RepLoopBackend — API Reference

**Base URL:** `http://localhost:5000` (API Gateway)

**Auth Header:** `Authorization: Bearer <token>`

**Tüm tarihler UTC ve ISO 8601 formatında:** `2026-03-04T14:30:00Z`

---

## 1. AuthService — `/api/auth`

### POST `/api/auth/register`

**Auth:** -

```json
// Request
{ "email": "string", "password": "string", "displayName": "string" }

// Response 200
{
  "token": "string",
  "refreshToken": "string",
  "user": {
    "id": "guid",
    "name": "string",
    "email": "string",
    "avatarUrl": "string|null",
    "isProfileComplete": false
  }
}
```

### POST `/api/auth/login`

**Auth:** -

```json
// Request
{ "email": "string", "password": "string" }

// Response 200 → AuthResult (aynı format)
```

### POST `/api/auth/refresh-token`

**Auth:** -

```json
// Request
{ "refreshToken": "string" }

// Response 200 → AuthResult
```

### POST `/api/auth/logout`

**Auth:** -

```json
// Request
{ "refreshToken": "string" }

// Response 200
```

### POST `/api/auth/forgot-password`

**Auth:** -

```json
// Request
{ "email": "string" }

// Response 200
{ "message": "Eğer bu email ile bir hesap varsa, şifre sıfırlama bağlantısı gönderildi." }
```

### POST `/api/auth/reset-password`

**Auth:** -

```json
// Request
{ "email": "string", "token": "string", "newPassword": "string" }

// Response 200
{ "message": "Şifreniz başarıyla sıfırlandı." }
```

### POST `/api/auth/change-password`

**Auth:** JWT

```json
// Request
{ "currentPassword": "string", "newPassword": "string" }

// Response 200
{ "message": "Şifreniz başarıyla değiştirildi." }
```

### POST `/api/auth/google`

**Auth:** -

```json
// Request
{ "idToken": "string" }

// Response 200 → AuthResult
```

### POST `/api/auth/apple`

**Auth:** -

```json
// Request
{ "identityToken": "string", "fullName": "string|null" }

// Response 200 → AuthResult
```

---

## 2. WorkoutService — `/api/workouts`

> Tüm endpointler JWT gerektirir.

### GET `/api/workouts?page=1&pageSize=20`

```json
// Response 200
{
  "items": [
    {
      "id": "guid",
      "name": "string",
      "description": "string|null",
      "notes": "string|null",
      "scheduledDate": "datetime|null",
      "durationMinutes": 0,
      "createdAt": "datetime",
      "exercises": [
        {
          "id": "guid",
          "exerciseId": "guid",
          "exerciseName": "string",
          "sets": 0,
          "reps": 0,
          "weightKg": 0.0,
          "durationSeconds": 0,
          "notes": "string|null"
        }
      ]
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 20,
  "totalPages": 0
}
```

### GET `/api/workouts/history?page=1&pageSize=10`

```json
// Response 200 → aynı format (WorkoutListDto)
```

### GET `/api/workouts/{id}`

```json
// Response 200 → WorkoutDto (yukarıdaki items içindeki obje)
// Response 404
```

### POST `/api/workouts`

```json
// Request
{
  "name": "string",
  "description": "string|null",
  "notes": "string|null",
  "scheduledDate": "datetime|null",
  "durationMinutes": 0,
  "exercises": [
    {
      "exerciseId": "guid",
      "exerciseName": "string",
      "sets": 0,
      "reps": 0,
      "weightKg": 0.0,
      "durationSeconds": 0,
      "notes": "string|null"
    }
  ]
}

// Response 201
{ "id": "guid" }
```

### PUT `/api/workouts/{id}`

```json
// Request (id route param ile eşleşmeli)
{
  "id": "guid",
  "name": "string",
  "description": "string|null",
  "notes": "string|null",
  "scheduledDate": "datetime|null",
  "durationMinutes": 0,
  "exercises": [
    {
      "exerciseId": "guid",
      "exerciseName": "string",
      "sets": 0,
      "reps": 0,
      "weightKg": 0.0,
      "durationSeconds": 0,
      "notes": "string|null"
    }
  ]
}

// Response 204
```

### DELETE `/api/workouts/{id}`

```json
// Response 204
```

---

## 3. ExerciseService — `/api/exercises`

> GET endpointleri public, POST/PUT/DELETE JWT gerektirir.

### GET `/api/exercises?muscleGroup=&equipment=&difficulty=&page=1&pageSize=20`

**Auth:** -

```json
// Response 200
{
  "items": [
    {
      "id": "guid",
      "name": "string",
      "description": "string|null",
      "muscleGroup": "string|null",
      "equipment": "string|null",
      "difficulty": "string|null",
      "videoUrl": "string|null",
      "imageUrl": "string|null",
      "isPublic": true,
      "createdAt": "datetime"
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 20,
  "totalPages": 0
}
```

### GET `/api/exercises/{id}`

**Auth:** -

```json
// Response 200 → ExerciseDto
// Response 404
```

### POST `/api/exercises`

**Auth:** JWT

```json
// Request
{
  "name": "string",
  "description": "string|null",
  "muscleGroup": "string|null",
  "equipment": "string|null",
  "difficulty": "string|null",
  "videoUrl": "string|null",
  "imageUrl": "string|null",
  "isPublic": true
}

// Response 201
{ "id": "guid" }
```

### PUT `/api/exercises/{id}`

**Auth:** JWT

```json
// Request
{
  "id": "guid",
  "name": "string",
  "description": "string|null",
  "muscleGroup": "string|null",
  "equipment": "string|null",
  "difficulty": "string|null",
  "videoUrl": "string|null",
  "imageUrl": "string|null",
  "isPublic": true
}

// Response 204
```

### DELETE `/api/exercises/{id}`

**Auth:** JWT

```json
// Response 204
```

---

## 4. UserService — `/api/user`

> Tüm endpointler JWT gerektirir. GET `/profile` ilk çağrıda profili otomatik oluşturur.

### GET `/api/user/profile`

```json
// Response 200
{
  "userId": "guid",
  "displayName": "string",
  "age": 0,
  "heightCm": 0,
  "weightKg": 0.0,
  "experienceLevel": "Beginner|Intermediate|Advanced",
  "goal": "WeightLoss|MuscleGain|Endurance|Flexibility|GeneralFitness",
  "avatarUrl": "string|null"
}
```

### PUT `/api/user/profile`

```json
// Request
{
  "displayName": "string",
  "age": 0,
  "heightCm": 0,
  "weightKg": 0.0,
  "experienceLevel": "Beginner|Intermediate|Advanced",
  "goal": "WeightLoss|MuscleGain|Endurance|Flexibility|GeneralFitness"
}

// Response 200 → UserProfileDto
```

### PUT `/api/user/avatar`

**Content-Type:** `multipart/form-data`

| Alan | Tip | Kısıt |
|------|-----|-------|
| `file` | binary | Max 5MB, JPEG/PNG/WebP |

```json
// Response 200
{ "avatarUrl": "string" }

// Response 400
{ "error": "Dosya seçilmedi." }
{ "error": "Sadece JPEG, PNG ve WebP formatları desteklenir." }
```

### DELETE `/api/user/account`

```json
// Response 204
```

---

## 5. SettingsService — `/api/settings`

> Tüm endpointler JWT gerektirir. GET `/settings` ilk çağrıda ayarları otomatik oluşturur.
> PATCH endpointleri partial update — sadece gönderilen alanlar güncellenir (nullable alanlar).

### GET `/api/settings`

```json
// Response 200
{
  "workout": {
    "weightUnit": "Kg|Lb",
    "distanceUnit": "Km|Miles",
    "defaultDurationMinutes": 0,
    "restBetweenSetsSeconds": 0,
    "workoutDays": ["Monday", "Wednesday"]
  },
  "notifications": {
    "emailNotifications": true,
    "pushNotifications": true,
    "workoutReminders": true,
    "weeklyReport": true,
    "achievementAlerts": true
  },
  "privacy": {
    "allowDataAnalysis": true
  }
}
```

### PATCH `/api/settings/workout`

```json
// Request (tüm alanlar nullable — sadece gönderilenler güncellenir)
{
  "weightUnit": "Kg|Lb",
  "distanceUnit": "Km|Miles",
  "defaultDurationMinutes": 0,
  "restBetweenSetsSeconds": 0,
  "workoutDays": ["Monday", "Wednesday", "Friday"]
}

// Response 200 → SettingsDto (tam obje)
```

### PATCH `/api/settings/notifications`

```json
// Request (tüm alanlar nullable)
{
  "emailNotifications": true,
  "pushNotifications": true,
  "workoutReminders": true,
  "weeklyReport": true,
  "achievementAlerts": true
}

// Response 200 → SettingsDto
```

### PATCH `/api/settings/privacy`

```json
// Request (nullable)
{ "allowDataAnalysis": true }

// Response 200 → SettingsDto
```

---

## 6. SessionService — `/api/sessions`

> Tüm endpointler JWT gerektirir.

### POST `/api/sessions`

```json
// Request
{ "workoutId": "guid", "workoutName": "string" }

// Response 201
{ "id": "guid" }
```

### GET `/api/sessions/history?page=1&pageSize=10`

> Yalnızca `Completed` oturumları döner.

```json
// Response 200
{
  "items": [SessionDto],
  "totalCount": 0,
  "page": 1,
  "pageSize": 10
}
```

### GET `/api/sessions/{id}`

```json
// Response 200
{
  "id": "guid",
  "workoutId": "guid",
  "workoutName": "string",
  "status": "Active|Paused|Completed|Abandoned",
  "startedAt": "datetime",
  "pausedAt": "datetime|null",
  "completedAt": "datetime|null",
  "totalDurationSeconds": 0,
  "notes": "string|null",
  "sets": [
    {
      "id": "guid",
      "exerciseId": "guid",
      "exerciseName": "string",
      "setNumber": 0,
      "reps": 0,
      "weightKg": 0.0,
      "durationSeconds": 0,
      "notes": "string|null",
      "completedAt": "datetime"
    }
  ]
}

// Response 404
```

### POST `/api/sessions/{id}/sets`

```json
// Request
{
  "exerciseId": "guid",
  "exerciseName": "string",
  "setNumber": 0,
  "reps": 0,
  "weightKg": 0.0,
  "durationSeconds": 0,
  "notes": "string|null"
}

// Response 200
{ "id": "guid" }
```

### PATCH `/api/sessions/{id}`

```json
// Request
{ "action": "Pause|Resume" }

// Response 204
```

### POST `/api/sessions/{id}/complete`

```json
// Request
{ "notes": "string|null" }

// Response 204
```

---

## 7. StatisticsService — `/api/statistics`

> Tüm endpointler JWT gerektirir.

### POST `/api/statistics/exercise-logs`

> Antrenman tamamlandığında set verilerini kaydeder. Strength graph ve personal records buradan hesaplanır.

```json
// Request
{
  "exerciseId": "guid",
  "exerciseName": "string",
  "weightKg": 0.0,
  "reps": 0,
  "performedAt": "datetime"
}

// Response 200
{ "id": "guid" }
```

### GET `/api/statistics/strength?exerciseId={id}&period=30d`

> Period formatları: `7d` (gün), `4w` (hafta), `3m` (ay). Default: `30d`

```json
// Response 200
{
  "exerciseId": "guid",
  "exerciseName": "string",
  "dataPoints": [
    {
      "date": "datetime",
      "maxWeightKg": 0.0,
      "maxReps": 0,
      "totalVolume": 0.0
    }
  ]
}
```

### GET `/api/statistics/personal-records`

```json
// Response 200
[
  {
    "exerciseId": "guid",
    "exerciseName": "string",
    "maxWeightKg": 0.0,
    "maxReps": 0,
    "achievedAt": "datetime"
  }
]
```

### POST `/api/statistics/body-measurements`

> Tüm ölçüm alanları nullable — sadece `measuredAt` zorunlu.

```json
// Request
{
  "measuredAt": "datetime",
  "weightKg": 0.0,
  "bodyFatPercentage": 0.0,
  "chestCm": 0.0,
  "waistCm": 0.0,
  "hipsCm": 0.0,
  "bicepsCm": 0.0,
  "thighCm": 0.0,
  "notes": "string|null"
}

// Response 201
{ "id": "guid" }
```

### GET `/api/statistics/body-measurements?page=1&pageSize=10`

```json
// Response 200
{
  "items": [
    {
      "id": "guid",
      "measuredAt": "datetime",
      "weightKg": 0.0,
      "bodyFatPercentage": 0.0,
      "chestCm": 0.0,
      "waistCm": 0.0,
      "hipsCm": 0.0,
      "bicepsCm": 0.0,
      "thighCm": 0.0,
      "notes": "string|null"
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 10
}
```

---

## Hata Formatı

Tüm servisler aynı hata formatını kullanır:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110",
  "title": "Bad Request",
  "status": 400,
  "detail": "Hata açıklaması",
  "errorCode": "XXX-0000"
}
```

### HTTP Status Kodları

| Status | Açıklama |
|--------|----------|
| `200` | Başarılı |
| `201` | Oluşturuldu (Location header ile) |
| `204` | Başarılı, içerik yok |
| `400` | Validation hatası / kötü istek |
| `401` | Token eksik veya geçersiz |
| `403` | Yetkisiz erişim |
| `404` | Kayıt bulunamadı |
| `409` | Conflict (ör. session zaten tamamlanmış) |

### Validation Hatası (422)

```json
{
  "type": "https://tools.ietf.org/html/rfc9110",
  "title": "Validation Failed",
  "status": 422,
  "errors": {
    "Name": ["'Name' must not be empty."],
    "Email": ["'Email' is not a valid email address."]
  }
}
```

### Hata Kodları

| Prefix | Servis |
|--------|--------|
| `AUTH-1xxx` | AuthService |
| `WRK-2xxx` | WorkoutService |
| `EXR-3xxx` | ExerciseService |
| `USR-4xxx` | UserService |
| `SET-5xxx` | SettingsService |
| `SES-6xxx` | SessionService |
| `STAT-7xxx` | StatisticsService |

---

## 8. RecommenderService — `/api/recommendations`

> Python/FastAPI tabanlı içerik bazlı öneri motoru. composite-scoring-v3 algoritması kullanır.

### POST `/api/recommendations/`

**Auth:** -

```json
// Request
{
  "user_id": "guid",
  "experience_level": "Beginner|Intermediate|Advanced",
  "goal": "WeightLoss|MuscleGain|Endurance|Flexibility|GeneralFitness"
}

// Query Params
// top_n: int (1-20, default 5)

// Response 200
{
  "user_id": "guid",
  "algorithm": "composite-scoring-v3",
  "recommendations": [
    {
      "workout_id": "guid",
      "workout_name": "string",
      "description": "string|null",
      "duration_minutes": 0,
      "exercise_count": 0,
      "muscle_groups": ["Chest", "Shoulders"],
      "score": 0.85,
      "reason": "string",
      "tags": ["Orta", "Chest", "60 dk", "6 hareket"]
    }
  ]
}
```

### Scoring Sinyalleri (v3)

| Sinyal | Ağırlık | Açıklama |
|--------|---------|----------|
| Difficulty Match | 0.25 | Egzersiz zorluk dağılımı ile kullanıcı seviyesi uyumu |
| Muscle Variety | 0.25 | Son 4 günde çalışılan kas gruplarıyla çakışma penaltisi |
| Duration Match | 0.15 | Antrenman süresi ile seviyeye uygun ideal süre aralığı |
| Volume Match | 0.15 | Toplam set hacmi ile seviyeye uygun hacim aralığı |
| Variety | 0.10 | Yakın zamanda yapılmamış workout'lara bonus |
| Recency Boost | 0.10 | Daha önce tamamlanmış workout'lara bonus |

### GET `/api/recommendations/health`

```json
// Response 200
{ "status": "ok", "service": "reploop-recommender" }
```

---

## Gateway Route Tablosu

| Route | Servis | Port |
|-------|--------|------|
| `/api/auth/**` | AuthService | 5174 |
| `/api/workouts/**` | WorkoutService | 5175 |
| `/api/exercises/**` | ExerciseService | 5176 |
| `/api/user/**` | UserService | 5177 |
| `/api/settings/**` | SettingsService | 5178 |
| `/api/sessions/**` | SessionService | 5179 |
| `/api/statistics/**` | StatisticsService | 5180 |
| `/api/recommendations/**` | RecommenderService | 5181 |
