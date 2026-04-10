# QuizApi

A RESTful Web API for a quiz platform built with ASP.NET Core 8. Supports user authentication, quiz creation, and result tracking.

🚀 **Live API:** https://quiz-api-401374266526.europe-central2.run.app/api/quizzes

---

## Tech Stack

- **ASP.NET Core 8** — Web API framework
- **Entity Framework Core 8** — ORM
- **SQLite** — Database
- **JWT Bearer** — Authentication
- **FluentValidation** — Request validation
- **BCrypt.Net** — Password hashing
- **Swagger / Swashbuckle** — API documentation
- **xUnit + Moq + FluentAssertions** — Testing

---

## Architecture

The project follows a clean layered architecture:

Controllers         → HTTP layer, inherits BaseController
Services            → Business logic (interfaces + implementations)
Repositories        → Data access (interfaces + implementations)
Validators          → FluentValidation validators per DTO
DTOs                → Request/Response data transfer objects
Common              → Result<T>, ApiResponse<T>, Error pattern
Models              → EF Core entities
Data                → AppDbContext, IAppDbContext

All responses follow a unified structure:

```json
{
  "success": true,
  "message": "Success",
  "data": { }
}
```

---

## API Endpoints

### Auth
| Method |       Endpoint       |        Description        | Auth |
|--------|----------------------|---------------------------|------|
|  POST  | `/api/auth/register` | Register new account      |  —   |
|  POST  | `/api/auth/login`    | Login and get JWT token   |  —   |
|  GET   | `/api/auth/me`       | Get current user profile  |  ✓   |
|  PUT   | `/api/auth/me`       | Update profile / password |  ✓   |

### Quizzes
| Method |       Endpoint       |          Description           | Auth |
|--------|----------------------|--------------------------------|------|
|  GET   | `/api/quizzes`       | Search quizzes with pagination |  —   |
|  GET   | `/api/quizzes/top`   | Get top quizzes by play count  |  —   |
|  GET   | `/api/quizzes/my`    | Get my quizzes                 |  ✓   |
|  GET   | `/api/quizzes/{id}`  | Get quiz with questions        |  —   |
|  POST  | `/api/quizzes`       | Create quiz                    |  ✓   |
|  PUT   | `/api/quizzes/{id}`  | Update quiz                    | ✓ Owner |
| DELETE | `/api/quizzes/{id}`  | Delete quiz                    | ✓ Owner |

### Results
| Method |            Endpoint                 |    Description     | Auth |
|--------|-------------------------------------|--------------------|------|
|  POST  | `/api/results`                      | Submit quiz result |  ✓   |
|  GET   | `/api/results/my`                   | Get my results     |  ✓   |
|  GET   | `/api/results/leaderboard/{quizId}` | Get leaderboard    |  —   |

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run locally

```bash
git clone https://github.com/saturnnn7/quiz-app-api.git
cd quiz-app-api

dotnet restore
dotnet ef database update
dotnet run
```

API will be available at `http://localhost:5191`  
Swagger UI at `http://localhost:5191/swagger`

### Run tests

```bash
dotnet test
```

---

## Deployment

Deployed on **Google Cloud Run** using Docker.

```bash
gcloud run deploy quiz-api \
  --source . \
  --platform managed \
  --region europe-central2 \
  --allow-unauthenticated \
  --port 8080
```

---

## Database Schema

|   Table   |             Description                    |
|-----------|--------------------------------------------|
| Users     | User accounts with hashed passwords        |
| Quizzes   | Quiz metadata (title, description, author) |
| Questions | 4-option questions linked to quizzes       |
| Results   | Quiz completion records with scores        |