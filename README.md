# Yuu.Eip

A multi-tenant enterprise integration platform built on ABP Framework with Domain-Driven Design (DDD) layered architecture.

## Tech Stack

- **Backend:** ASP.NET Core 10 (.NET 10) + ABP Framework 10.0.2
- **Frontend:** Angular 20 + Lepton X Theme
- **Database:** PostgreSQL
- **Authentication:** OpenIddict (OAuth2/OIDC)

## Prerequisites

- [.NET 10.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node.js v20.11+](https://nodejs.org/en)
- [PostgreSQL](https://www.postgresql.org/)

## Quick Start

### 1. Initialize Database

Run the database migrator to create the database and seed initial data:

```bash
cd aspnet-core
dotnet run --project src/Yuu.Eip.DbMigrator
```

### 2. Start Backend API

```bash
cd aspnet-core
dotnet run --project src/Yuu.Eip.HttpApi.Host
```

The API will be available at `https://localhost:44319`.

### 3. Start Frontend

```bash
cd angular
npm install
npm start
```

The frontend will be available at `http://localhost:4200`.

## Project Structure

```
yuu-eip-api/
├── aspnet-core/                 # Backend ASP.NET Core projects
│   ├── src/
│   │   ├── Yuu.Eip.Domain/              # Domain layer: entities, aggregates, domain services
│   │   ├── Yuu.Eip.Domain.Shared/       # Shared layer: constants, enums, shared DTOs
│   │   ├── Yuu.Eip.Application/         # Application layer: app services, business logic
│   │   ├── Yuu.Eip.Application.Contracts/ # Application contracts: service interfaces, DTOs
│   │   ├── Yuu.Eip.EntityFrameworkCore/ # Infrastructure layer: DbContext, repositories
│   │   ├── Yuu.Eip.HttpApi/             # API layer: controllers
│   │   ├── Yuu.Eip.HttpApi.Host/        # Host: application entry point
│   │   └── Yuu.Eip.DbMigrator/          # Database migration tool
│   └── test/                    # Test projects
│
├── angular/                     # Frontend Angular project
│   └── src/app/
│       ├── home/                # Home module
│       ├── human-resources/     # Human resources module
│       ├── proxy/               # API proxy services (auto-generated)
│       └── shared/              # Shared components
│
└── .github/workflows/           # CI/CD pipelines
```

## Common Commands

### Backend

```bash
dotnet build                     # Build solution
dotnet test                      # Run tests
dotnet ef migrations add <Name> -p src/Yuu.Eip.EntityFrameworkCore -s src/Yuu.Eip.HttpApi.Host  # Add migration
```

### Frontend

```bash
npm start                        # Development server
npm run build:prod               # Production build
npm test                         # Run tests
npm run lint                     # Lint check
```

### Docker

```bash
docker build -t yuu-eip -f aspnet-core/Dockerfile aspnet-core/
```

## Configuration

| File | Description |
|------|-------------|
| `aspnet-core/src/Yuu.Eip.HttpApi.Host/appsettings.json` | Backend main configuration |
| `aspnet-core/Directory.Packages.props` | NuGet package version management |
| `angular/angular.json` | Angular project configuration |
| `angular/src/environments/` | Frontend environment settings |

## Documentation

- [Backend Development Guide](aspnet-core/README.md)
- [Frontend Development Guide](angular/README.md)

## Environment & Configuration

Backend settings live in `aspnet-core/src/Yuu.Eip.HttpApi.Host/appsettings*.json`. Common items to verify:
- `ConnectionStrings:Default` (PostgreSQL)
- OpenIddict signing/encryption certificate path and password
- Optional logging endpoints (e.g., Seq)

Frontend environment settings live in `angular/src/environments/`.

## Development Flow (Local)

1. `dotnet run --project aspnet-core/src/Yuu.Eip.DbMigrator`
2. `dotnet run --project aspnet-core/src/Yuu.Eip.HttpApi.Host`
3. `cd angular && npm install && npm start`

## Frontend ↔ Backend Integration

The Angular app targets the API base URL defined in `angular/src/environments/`. If a proxy is used, document the proxy file path and expected port.

## Initial Data / Accounts

If you seed a default tenant/admin account via `DbMigrator`, document the default credentials and how to change them.

## Troubleshooting

- HTTPS dev cert issues: run `dotnet dev-certs https --trust`
- OpenIddict cert missing/invalid: see `aspnet-core/README.md` for generating `openiddict.pfx`
- Migration errors: re-run `DbMigrator` after verifying the connection string

## License

Private project. All rights reserved.
