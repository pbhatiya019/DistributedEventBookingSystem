# Distributed Event Booking System

A cloud-ready, microservices-based event booking platform built with ASP.NET Core. The system is composed of three independent services (Auth, Event, Ticket) that communicate over HTTP, containerized with Docker, deployed to Azure App Service, and backed by a CI/CD pipeline via GitHub Actions.

## Architecture

```
+-------------+     +--------------+     +---------------+
| AuthService |     | EventService |     | TicketService |
| JWT + SQLite|     | CRUD Events  |     | Book / Cancel |
+-------------+     +--------------+     +---------------+
       |                   |                     |
       +-------------------+---------------------+
                           |
                     ClientApp (React)
```

## Services

| Service | Responsibility | Tech |
|---------|---------------|------|
| **AuthService** | User registration, login, JWT issuance | ASP.NET Core, SQLite, BCrypt |
| **EventService** | Create, list, and manage events | ASP.NET Core, EF Core |
| **TicketService** | Book and cancel tickets for events | ASP.NET Core, EF Core |
| **ClientApp** | Web frontend for end users | React, TypeScript |

## Tech Stack

- **Backend:** C# / ASP.NET Core (.NET 8)
- **Auth:** JWT Bearer Tokens
- **Database:** SQLite (dev), configurable via EF Core
- **Containerization:** Docker + Docker Compose
- **Cloud:** Azure App Service
- **CI/CD:** GitHub Actions
- **Testing:** xUnit - unit tests for token generation and DTO validation

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run with Docker Compose

```bash
git clone https://github.com/pbhatiya019/DistributedEventBookingSystem.git
cd DistributedEventBookingSystem
docker-compose up --build
```

All three services will start on their respective ports as defined in `docker-compose.yml`.

### Run Locally (without Docker)

```bash
# Start each service in a separate terminal
cd AuthService && dotnet run
cd EventService && dotnet run
cd TicketService && dotnet run
cd ClientApp && npm install && npm start
```

### Run Tests

```bash
cd DistributedEventBookingSystem.Tests
dotnet test
```

## Project Structure

```
DistributedEventBookingSystem/
├── .github/workflows/      # CI/CD pipeline (GitHub Actions)
├── AuthService/            # Authentication microservice
├── EventService/           # Event management microservice
├── TicketService/          # Ticket booking microservice
├── ClientApp/              # React frontend
├── DistributedEventBookingSystem.Tests/  # Unit tests
├── docker-compose.yml      # Multi-container orchestration
└── DistributedEventBookingSystem.slnx   # Solution file
```

## CI/CD

GitHub Actions workflows automatically build, test, and deploy each service to Azure App Service on push to `main`.

## Author

**Pratham Bhatiya** - [LinkedIn](https://www.linkedin.com/in/pratham-bhatiya-14064b327) | [GitHub](https://github.com/pbhatiya019)
