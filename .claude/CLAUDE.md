# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Yuu.Eip is a full-stack multi-tenant enterprise platform built on the **ABP Framework** (v10.0.2) with a layered monolithic architecture following Domain-Driven Design (DDD) principles.

**Stack:** ASP.NET Core 10 (.NET 10) + Angular 20 + PostgreSQL + OpenIddict (OAuth2/OIDC)

## Build & Development Commands

### Backend (aspnet-core/)

```bash
dotnet restore                                      # Restore dependencies
dotnet build                                        # Build solution
dotnet run --project src/Yuu.Eip.DbMigrator         # Apply migrations & seed data (first run)
dotnet run --project src/Yuu.Eip.HttpApi.Host       # Run API host

# Testing
dotnet test                                         # Run all tests
dotnet test test/Yuu.Eip.Application.Tests          # Run specific test project
dotnet test --filter "FullyQualifiedName~EmployeeAppServiceTests"  # Run specific test class
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"  # Run single test

# Database migrations
dotnet ef migrations add <Name> -p src/Yuu.Eip.EntityFrameworkCore -s src/Yuu.Eip.HttpApi.Host
dotnet ef database update -p src/Yuu.Eip.EntityFrameworkCore -s src/Yuu.Eip.HttpApi.Host

# ABP CLI (run in HttpApi.Host directory)
abp install-libs                                    # Install client-side libraries
```

### Frontend (angular/)

```bash
npm install                    # Install dependencies
npm start                      # Dev server at http://localhost:4200
npm run build:prod             # Production build
npm test                       # Run Karma/Jasmine tests (watch mode)
npm run test -- --watch=false  # Run tests without watch (CI mode)
npm run test -- --include="**/component-name.spec.ts"  # Run specific test file
npm run lint                   # ESLint check
npm run ng -- generate component <name>   # Generate Angular component
npm run ng -- generate service <name>     # Generate Angular service
```

### Docker

```bash
docker build -t yuu-eip -f aspnet-core/Dockerfile aspnet-core/
```

## Architecture

### Backend Layers (aspnet-core/src/)

| Layer | Purpose |
|-------|---------|
| `Yuu.Eip.Domain.Shared` | Constants, enums, shared contracts |
| `Yuu.Eip.Domain` | Entities, aggregates, domain services |
| `Yuu.Eip.Application.Contracts` | Service interfaces, DTOs |
| `Yuu.Eip.Application` | Application services, business logic |
| `Yuu.Eip.EntityFrameworkCore` | DbContext, migrations, repositories |
| `Yuu.Eip.HttpApi` | API controllers |
| `Yuu.Eip.HttpApi.Client` | Client SDK for API consumption |
| `Yuu.Eip.HttpApi.Host` | Main host application, module configuration |

### Frontend Structure (angular/src/app/)

- `app.config.ts` - Angular configuration with ABP providers
- `app.routes.ts` - Routing configuration
- `route.provider.ts` - Dynamic route registration
- `home/` - Home module
- `human-resources/` - HR modules (employees, departments, positions)
- `proxy/` - Generated API proxy services
- `shared/` - Shared components and services

## Code Style

### C# Naming
- **Classes/Methods/Properties**: PascalCase (`EmployeeAppService`, `GetAsync`)
- **Private fields**: _camelCase (`_employeeRepository`)
- **Interfaces**: `I` prefix (`IEmployeeAppService`)
- Async methods end with `Async`

### TypeScript Naming
- **Components**: PascalCase + `Component` suffix (`EmployeeListComponent`)
- **Services**: PascalCase + `Service` suffix (`EmployeeService`)
- **Methods/Properties**: camelCase (`getEmployees`, `employeeId`)
- Use single quotes, standalone components (Angular 20+)

### Testing Conventions
- **Backend test naming**: `[MethodName]_[Scenario]_[ExpectedResult]`
- **Frontend test files**: `*.spec.ts` alongside components

## Key Configuration

- **Database connection:** `aspnet-core/src/Yuu.Eip.HttpApi.Host/appsettings.json`
- **Package versions:** `aspnet-core/Directory.Packages.props` (centralized management)
- **NuGet sources:** `aspnet-core/NuGet.Config` (includes GitHub Packages for Yuu.* packages)
- **Angular config:** `angular/angular.json`
- **Code formatting:** `.editorconfig` (4 spaces for C#, 2 spaces for TS/JS/HTML/SCSS)

## Custom Dependencies

The project uses custom `Yuu.*` NuGet packages from GitHub Packages:
- `Yuu.AspNetCore.Hosting/Middleware/Mvc`
- `Yuu.Serilog.Seq`
- `Yuu.Abp.EntityFrameworkCore`

Docker builds require `NUGET_GITHUB_TOKEN` secret for authentication.

## CI/CD

- **GitHub Actions** (`.github/workflows/docker-build-push.yml`): Builds and pushes to Docker Hub on `stag` branch or version tags

## Testing

- **Backend:** xUnit + NSubstitute + Shouldly + SQLite (for EF Core tests)
- **Frontend:** Karma + Jasmine

## Commit Style

Use Conventional Commits: `type: short description`
- Prefixes: `fix:`, `feat:`, `chore:`, `ci:`, `docs:`, `refactor:`
- Include UI screenshots for Angular changes in PRs

## Notes

- Run `Yuu.Eip.DbMigrator` before first application run to set up database
- OpenIddict requires `openiddict.pfx` certificate for OAuth2 operations
- Custom `IPWhitelistMiddleware` is used for API security
- Uses custom build image `ghcr.io/yy556023/abp-sdk:10.0` in Docker builds
- Local secrets: `appsettings.local-dev.json`, `appsettings.secrets.json` (not committed)
