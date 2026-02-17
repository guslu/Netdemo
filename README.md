# Netdemo SaaS Project Management Platform

Production-ready baseline for a .NET 8 + React TypeScript SaaS project management system.

## Architecture decisions
- **Clean Architecture** across `Api`, `Application`, `Domain`, `Infrastructure` and dedicated tests projects.
- **CQRS-lite with MediatR** keeps use cases explicit while avoiding unnecessary complexity.
- **Centralized validation with FluentValidation + pipeline behavior** ensures commands/queries are validated consistently.
- **ASP.NET Identity + JWT + role authorization** provides enterprise-ready authentication and authorization.
- **Tenant-aware request handling** via organization claim checks in application handlers.
- **ProblemDetails + global exception handler** standardizes API error responses.
- **Audit hooks** are recorded for core write operations (projects, work items, comments).

## Included capabilities
- JWT login and admin-only user registration endpoint.
- Role-based access control (`Admin`, `Manager`, `Member`).
- Organizations, projects, work items, comments, and audit log persistence model.
- Health endpoints (`/health/live`, `/health/ready`).
- API versioning (`v1`).
- Serilog structured logging.
- React frontend with login flow, protected routes, project/work-item views, and API integration.
- Docker + docker-compose for API and SQL Server.
- CI scaffold for build, tests, coverage, and Docker build.

## Solution layout
- `src/Api` - HTTP entrypoint, middleware, auth policies, health/versioning.
- `src/Application` - commands/queries, validation, handlers, abstractions.
- `src/Domain` - entities and core business primitives.
- `src/Infrastructure` - EF Core, Identity, JWT, persistence and service implementations.
- `tests/UnitTests` + `tests/IntegrationTests` - backend test projects.
- `frontend` - React + TypeScript + Vite + Tailwind frontend.

## Running with Docker Compose
1. Set env vars:
   - `DB_PASSWORD`
   - `JWT_SIGNING_KEY`
2. Run:
   ```bash
   docker compose up --build
   ```

## Cloud-readiness hooks
- Environment-based appsettings and env-var overrides.
- `ApplicationInsights:ConnectionString` config slot.
- `AzureKeyVault:VaultUri` config slot ready for provider wiring.
- HTTPS + HSTS defaults and CORS policy configuration.
