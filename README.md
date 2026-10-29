# Keeper API

ASP.NET Core Web API для взаимодействия посетителей с Хранителем.

## Запуск

```bash
# 1. PostgreSQL
docker run -d --name keeper-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=keeper_db -p 5432:5432 postgres:17-alpine

# 2. Приложение
dotnet run

# 3. Swagger: http://localhost:5074
```

## Эндпоинты

| Метод | Роут | Описание |
|-------|------|----------|
| POST | /api/visitors | Новый посетитель (trust=0) |
| GET | /api/visitors | Список посетителей |
| GET | /api/visitors/{id} | Посетитель по id |
| POST | /api/visitors/{id}/actions | Действие (Introduce/OfferHelp/Threaten) |
| GET | /api/visitors/{id}/access | Проверка доступа (trust >= 50) |
