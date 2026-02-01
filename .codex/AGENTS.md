# Repository Guidelines

## Project Structure & Module Organization
- `angular/` holds the Angular client (routes, components, and ABP Angular proxies). Main app code is in `angular/src/app`, assets in `angular/src/assets`, and environment settings in `angular/src/environments`.
- `aspnet-core/` contains the ABP-based backend. Core layers live under `aspnet-core/src/` (Domain, Application, EntityFrameworkCore, HttpApi, HttpApi.Host), while tests are under `aspnet-core/test/`.
- Database migrations are in `aspnet-core/src/Yuu.Eip.EntityFrameworkCore/Migrations`.

## Build, Test, and Development Commands
Frontend (run from `angular/`):
- `yarn install` (or `npm install`) installs dependencies.
- `yarn start` runs `ng serve --open` at `http://localhost:4200/`.
- `yarn build` or `yarn build:prod` builds the SPA.
- `yarn test` runs Karma/Jasmine unit tests.

Backend (run from `aspnet-core/`):
- `dotnet build` builds all projects.
- `dotnet run --project src/Yuu.Eip.HttpApi.Host/Yuu.Eip.HttpApi.Host.csproj` runs the API host.
- `dotnet run --project src/Yuu.Eip.DbMigrator/Yuu.Eip.DbMigrator.csproj` applies migrations and seeds data.
- `dotnet test Yuu.Eip.slnx` runs the test suite.

## Coding Style & Naming Conventions
- Formatting is enforced via `.editorconfig` in both roots. Indent with 2 spaces for TS/JS/JSON/HTML/SCSS, 4 spaces for C#.
- C# naming rules: interfaces start with `I`, types/members use PascalCase, private fields use `_camelCase`, and async methods end with `Async`.
- TypeScript uses single quotes.

## Testing Guidelines
- Backend tests use xUnit with Shouldly/NSubstitute; keep tests under `aspnet-core/test/` and follow existing naming (`*.Tests`).
- Frontend specs are `*.spec.ts` under `angular/src/app/` and run via Karma.

## Commit & Pull Request Guidelines
- Recent history uses Conventional Commit style prefixes like `chore:` and `ci:`; prefer `type: short description`.
- Keep commits focused; include a clear PR summary, test results, and link related issues. Add UI screenshots for Angular changes.

## Security & Configuration Tips
- Local settings live in `appsettings.local-dev.json` and `appsettings.secrets.json`; avoid committing real secrets.
- If OpenIddict certificates change, ensure `openiddict.pfx` aligns with deployment needs.
