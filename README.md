# RepLoopBackend

Fitness tracking microservices backend built with .NET 9, Clean Architecture, and CQRS (MediatR).

## Architecture

```
Client (React Native)
    │
    ▼
API Gateway (YARP :5000)
    ├── /api/auth/**      → AuthService     (:5174)
    ├── /api/workouts/**   → WorkoutService  (:5175)
    ├── /api/exercises/**  → ExerciseService (:5176)
    ├── /api/user/**       → UserService     (:5177)
    ├── /api/settings/**   → SettingsService (:5178)
    └── /api/sessions/**   → SessionService  (:5179)

AuthService ──(RabbitMQ)──► MailSenderService ──(SMTP)──► MailHog
```

## Services

| Service | Port | Description |
|---------|------|-------------|
| **API Gateway** | 5000 | YARP reverse proxy, CORS, rate limiting, health checks |
| **AuthService** | 5174 | JWT auth, registration, login, password reset, Google/Apple OAuth |
| **WorkoutService** | 5175 | Workout CRUD with exercises, history (paginated) |
| **ExerciseService** | 5176 | Exercise catalog with filters (muscle group, equipment, difficulty) |
| **UserService** | 5177 | User profile, avatar upload |
| **SettingsService** | 5178 | User preferences (units, notifications, privacy) |
| **SessionService** | 5179 | Live workout sessions, set logging, completion |
| **MailSenderService** | — | Background worker, consumes password reset events via RabbitMQ |

## Infrastructure

| Component | Port | Purpose |
|-----------|------|---------|
| PostgreSQL (AuthDB) | 5432 | Auth, Identity, RefreshTokens |
| PostgreSQL (WorkoutDB) | 5433 | Workouts, WorkoutExercises |
| PostgreSQL (ExerciseDB) | 5434 | Exercises |
| PostgreSQL (UserDB) | 5435 | UserProfiles |
| PostgreSQL (SettingsDB) | 5436 | UserSettings |
| PostgreSQL (SessionDB) | 5437 | WorkoutSessions, SessionSets |
| RabbitMQ | 5672 / 15672 | Event bus (AMQP / Management UI) |
| MailHog | 1025 / 8025 | Dev SMTP / Web UI |

## Quick Start

### Option 1: Full Docker (recommended)

```bash
docker compose up -d
```

This starts all infrastructure **and** all application services. The API Gateway is available at `http://localhost:5000`.

### Option 2: Infrastructure only + local services

```bash
# Start databases, RabbitMQ, MailHog
docker compose up -d

# Run services locally (each in a separate terminal)
dotnet run --project services/ApiGateway/src/ApiGateway
dotnet run --project services/AuthService/src/RepLoopBackend.API
dotnet run --project services/WorkoutService/src/WorkoutService.API
dotnet run --project services/ExerciseService/src/ExerciseService.API
dotnet run --project services/UserService/src/UserService.API
dotnet run --project services/SettingsService/src/SettingsService.API
dotnet run --project services/SessionService/src/SessionService.API
dotnet run --project services/MailSenderService/src/MailSenderService
```

## Configuration

Secrets are stored in `appsettings.Development.json` (gitignored). The committed `appsettings.json` contains `CHANGE_ME` placeholders.

After cloning, create `appsettings.Development.json` in each service's project directory with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=<port>;Database=<db>;Username=reploop;Password=reploop123"
  },
  "JwtSettings": {
    "SecretKey": "RepLoopBackend-SuperSecret-Key-Min32Chars!!"
  }
}
```

See `docker-compose.yml` for database port/name mappings per service.

## Health Checks

All services expose `GET /health`. The API Gateway aggregates them at its own `/health` endpoint.

## Project Structure

```
services/
├── Shared/
│   ├── RepLoopBackend.SharedKernel/    # Exceptions, Behaviors, ApiControllerBase, Swagger filter
│   └── RepLoopBackend.Contracts/       # Cross-service event contracts (MassTransit)
├── ApiGateway/
├── AuthService/
├── WorkoutService/
├── ExerciseService/
├── UserService/
├── SettingsService/
├── SessionService/
└── MailSenderService/
```

Each service follows Clean Architecture:

```
Service/src/
├── Service.Domain/          # Entities, enums, base classes
├── Service.Application/     # CQRS (MediatR), Manager classes, interfaces, DTOs
├── Service.Infrastructure/  # ICurrentUserService implementation
├── Service.Persistence/     # EF Core DbContext, migrations, DI
└── Service.API/             # Controllers, Program.cs, appsettings
```
