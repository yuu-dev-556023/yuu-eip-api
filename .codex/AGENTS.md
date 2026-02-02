# Repository Guidelines

## Project Structure & Module Organization
This repo is a layered ABP Framework solution with a separate Angular app:
- `aspnet-core/src/` contains the backend layers (Domain, Application, EF Core, HttpApi, Host).
- `aspnet-core/test/` contains backend test projects.
- `angular/src/app/` contains the frontend modules and shared UI.
- `.github/workflows/` holds CI workflows.

## Build, Test, and Development Commands
Run commands from the repo root unless noted:
- `dotnet run --project aspnet-core/src/Yuu.Eip.DbMigrator` initializes/updates the PostgreSQL database.
- `dotnet run --project aspnet-core/src/Yuu.Eip.HttpApi.Host` starts the API (default `https://localhost:44319`).
- `dotnet build` builds the backend solution.
- `dotnet test` runs all backend tests under `aspnet-core/test/`.
- `dotnet ef migrations add <Name> -p aspnet-core/src/Yuu.Eip.EntityFrameworkCore -s aspnet-core/src/Yuu.Eip.HttpApi.Host` adds a migration.
- `npm install` then `npm start` in `angular/` starts the Angular dev server (`http://localhost:4200`).
- `npm run build:prod`, `npm test`, `npm run lint` in `angular/` build, test, and lint the frontend.

## Coding Style & Naming Conventions
- Formatting is driven by `.editorconfig` files in `aspnet-core/` and `angular/`.
- C# uses 4-space indentation; JSON/TS/HTML/CSS use 2 spaces.
- C# naming: interfaces start with `I`, types/members are PascalCase, private fields use `_camelCase`, async methods end with `Async`.
- TypeScript prefers single quotes.

## Testing Guidelines
- Backend tests are xUnit-based (see `aspnet-core/test/`), run via `dotnet test`.
- Frontend unit tests run with Karma via `npm test` in `angular/`.
- No explicit coverage thresholds are configured; include tests for new business logic or API behavior.

## Commit & Pull Request Guidelines
- Recent history follows Conventional Commits (e.g., `fix: ...`, `chore: ...`). Keep the same `type: subject` format.
- PRs should include a short summary, linked issue (if any), and testing notes (e.g., `dotnet test`, `npm test`).
- Call out configuration or migration changes explicitly in the PR description.

## Configuration & Security Notes
- Backend settings live in `aspnet-core/src/Yuu.Eip.HttpApi.Host/appsettings*.json`.
- Frontend environment settings are in `angular/src/environments/`.
- OpenIddict signing cert guidance lives in `aspnet-core/README.md`; avoid committing secrets.
