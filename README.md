# Keeper API

ASP.NET Core Web API for modeling visitor interaction with the Keeper.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)

## Setup PostgreSQL

```bash
# Using Docker
docker run -d --name keeper-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=keeper_db -p 5432:5432 postgres:17-alpine

# Or use an existing PostgreSQL server
```

## Run the API

### Option 1: Direct local run

```bash
# Set the connection string (or use appsettings.json)
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=keeper_db;Username=postgres;Password=postgres"

# Run the app (Swagger opens automatically)
dotnet run
```

### Option 2: Docker Compose (app + database)

```bash
docker compose up -d --build
```

## API Overview

| Method | Route                    | Description                                  |
|--------|--------------------------|----------------------------------------------|
| POST   | /api/visitors            | Register a new visitor (starts at trust 0)   |
| GET    | /api/visitors            | List all visitors                            |
| GET    | /api/visitors/{id}       | Get a visitor by id                          |
| POST   | /api/visitors/{id}/actions | Perform an action (Introduce/OfferHelp/Threaten) |
| GET    | /api/visitors/{id}/access | Check if the visitor has access (trust >= 50) |

## Trust Mechanics

| Action     | Trust Change |
|------------|--------------|
| Introduce  | +20          |
| OfferHelp  | +30          |
| Threaten   | -20          |

- Trust is clamped between 0 and 100.
- Each action can be performed only once per visitor.
- Access is granted when trust is at least 50.
