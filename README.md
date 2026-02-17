# Netdemo SaaS Project Management Platform (Scaffold)

This repository now contains a production-oriented scaffold for a .NET 8 + React TypeScript SaaS platform.

## Architecture decisions
- **Clean Architecture** split into `Api`, `Application`, `Domain`, and `Infrastructure` to enforce direction of dependencies.
- **CQRS-lite with MediatR** to separate commands/queries while avoiding over-abstraction.
- **FluentValidation pipeline** for centralized request validation.
- **EF Core + ASP.NET Identity** for persistence and identity/security baseline.
- **ProblemDetails + global exception handling** for consistent API errors.
- **JWT + role-based authorization** for secure multi-role access.
- **Environment-based settings** and env var overrides for secrets and cloud deployment readiness.

## Current scope
This milestone only scaffolds structure and baseline wiring. Feature implementation (full auth flow, tenant isolation enforcement, richer business logic, migrations content, etc.) is intentionally deferred to next iterations.

## Solution layout
- `src/Api` - HTTP entrypoint, middleware, auth policies, health checks, API versioning.
- `src/Application` - use cases, validation, MediatR handlers, abstractions.
- `src/Domain` - core entities and business primitives.
- `src/Infrastructure` - EF Core, Identity, JWT generation, persistence wiring.
- `tests/UnitTests` / `tests/IntegrationTests` - baseline test suites.
- `frontend` - React + TypeScript + Vite + Tailwind scaffold with protected route and API client.

## Running with Docker Compose
1. Set env vars:
   - `DB_PASSWORD`
   - `JWT_SIGNING_KEY`
2. Run:
   ```bash
   docker compose up --build
   ```

## Cloud readiness hooks
- Application Insights connection string configuration slot in `appsettings`.
- Health endpoints: `/health/live` and `/health/ready`.
- Dockerized services for lift-and-shift to Azure App Service + Azure SQL.
