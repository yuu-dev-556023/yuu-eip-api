# AGENTS.md

This file provides guidance to agentic coding agents working with the Yuu.Eip codebase.

## Project Overview

Yuu.Eip is a full-stack multi-tenant enterprise platform built on the **ABP Framework** (v10.0.2) with a layered monolithic architecture following Domain-Driven Design (DDD) principles.

**Stack:** ASP.NET Core 10 (.NET 10) + Angular 20 + PostgreSQL + OpenIddict (OAuth2/OIDC)

## Build & Development Commands

### Backend (aspnet-core/)

```bash
# Build and restore
dotnet restore                                      # Restore dependencies
dotnet build                                        # Build solution

# Run applications
dotnet run --project src/Yuu.Eip.DbMigrator         # Apply migrations & seed data (first run)
dotnet run --project src/Yuu.Eip.HttpApi.Host       # Run API host

# Testing
dotnet test                                         # Run all tests
dotnet test test/Yuu.Eip.Application.Tests          # Run specific test project
dotnet test --filter "TestMethodName"              # Run single test method

# Database migrations
dotnet ef migrations add <Name> -p src/Yuu.Eip.EntityFrameworkCore -s src/Yuu.Eip.HttpApi.Host
dotnet ef database update -p src/Yuu.Eip.EntityFrameworkCore -s src/Yuu.Eip.HttpApi.Host

# ABP CLI (run in HttpApi.Host directory)
abp install-libs                                    # Install client-side libraries
```

### Frontend (angular/)

```bash
# Development
npm install                    # Install dependencies
npm start                      # Dev server at http://localhost:4200
npm run build:prod             # Production build

# Testing and linting
npm test                       # Run Karma/Jasmine tests
npm run test -- --watch=false  # Run tests without watch mode
npm run lint                   # ESLint check

# Code generation
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
- `human-resources/` - HR modules (employees, departments, positions, etc.)
- `shared/` - Shared components and services
- `proxy/` - Generated API proxy services

## Code Style Guidelines

### C# (Backend)

#### Naming Conventions
- **Classes**: PascalCase (e.g., `EmployeeAppService`)
- **Methods**: PascalCase (e.g., `GetAsync`, `CreateAsync`)
- **Properties**: PascalCase (e.g., `EmployeeNo`, `DepartmentId`)
- **Fields**: _camelCase with underscore prefix (e.g., `_employeeRepository`)
- **Constants**: PascalCase (e.g., `MaxResultCount`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IEmployeeAppService`)

#### Code Organization
- Use ABP's base classes: `EipAppService`, `FullAuditedAggregateRoot<Guid>`
- Apply authorization attributes: `[Authorize(EipPermissions.HumanResources.Employees.Default)]`
- Inject dependencies via constructor injection
- Use async/await patterns for database operations
- Follow DDD principles with separate layers

#### Error Handling
- Use ABP's built-in exception handling
- Throw `BusinessException` for domain rule violations
- Validate input in application services
- Use `Guard` clauses for null checks

#### Imports (using statements)
- System namespaces first (alphabetical)
- Microsoft namespaces second
- Third-party namespaces third
- Project namespaces last (alphabetical)

### TypeScript (Frontend)

#### Naming Conventions
- **Components**: PascalCase with `Component` suffix (e.g., `EmployeeListComponent`)
- **Services**: PascalCase with `Service` suffix (e.g., `EmployeeService`)
- **Methods**: camelCase (e.g., `getEmployees`, `createEmployee`)
- **Properties**: camelCase (e.g., `employeeId`, `departmentName`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_RESULTS`)
- **Interfaces**: PascalCase with optional `I` prefix (e.g., `Employee`, `IEmployee`)

#### Code Organization
- Use Angular decorators: `@Component`, `@Injectable`, `@NgModule`
- Implement ABP base components and services
- Use reactive forms with `FormBuilder`
- Follow Angular style guide patterns
- Use standalone components (Angular 20+)

#### Imports
- Angular core modules first
- Third-party libraries second
- ABP modules third
- Application modules last

## Testing Guidelines

### Backend Testing
- **Framework**: xUnit + NSubstitute + Shouldly
- **Test location**: `test/Yuu.Eip.*.Tests/`
- **Naming**: `[MethodName]_[Scenario]_[ExpectedResult]`
- Use `Shouldly` for assertions
- Mock dependencies with NSubstitute
- Test business logic, not infrastructure

### Frontend Testing
- **Framework**: Karma + Jasmine
- **Test location**: `*.spec.ts` files alongside components
- **Naming**: `describe` for component, `it` for specific behavior
- Mock services with jasmine spies
- Test component behavior, not DOM details

## Key Configuration

- **Database connection**: `aspnet-core/src/Yuu.Eip.HttpApi.Host/appsettings.json`
- **Package versions**: `aspnet-core/Directory.Packages.props` (centralized management)
- **NuGet sources**: `aspnet-core/NuGet.Config` (includes GitHub Packages for Yuu.* packages)
- **Angular config**: `angular/angular.json`
- **TypeScript config**: `angular/tsconfig.json`

## Custom Dependencies

The project uses custom `Yuu.*` NuGet packages from GitHub Packages:
- `Yuu.AspNetCore.Hosting/Middleware/Mvc`
- `Yuu.Serilog.Seq`
- `Yuu.Abp.EntityFrameworkCore`

Docker builds require `NUGET_GITHUB_TOKEN` secret for authentication.

## Important Notes

- Run `Yuu.Eip.DbMigrator` before first application run to set up database
- OpenIddict requires `openiddict.pfx` certificate for OAuth2 operations
- Custom `IPWhitelistMiddleware` is used for API security
- Uses custom build image `ghcr.io/yy556023/abp-sdk:10.0` in Docker builds
- Follow ABP conventions for permissions, localization, and settings
- Use multi-tenancy patterns throughout the application

## Running Single Tests

### Backend
```bash
# Run specific test class
dotnet test --filter "FullyQualifiedName~EmployeeAppServiceTests"

# Run specific test method
dotnet test --filter "FullyQualifiedName~EmployeeAppServiceTests.CreateAsync_ValidEmployee_ReturnsCreatedEmployee"
```

### Frontend
```bash
# Run specific test file
npm test -- --include="**/employee-list.component.spec.ts"

# Run tests without watch (CI mode)
npm run test -- --watch=false --browsers=ChromeHeadless
```