# MyErp

ERP application skeleton: an ASP.NET Core 9 modular monolith over PostgreSQL,
with an Angular 21 + PrimeNG 21 frontend.

No ERP features are implemented yet. This is the architectural foundation that
features get added to one at a time.

## Layout

```text
/
├── backend/            ASP.NET Core 9 modular monolith
│   ├── MyErp.sln
│   ├── MyErp.Api/          host and composition root only
│   ├── MyErp.Data/         DbContext, repositories, persistence infrastructure
│   ├── MyErp.Entities/     persisted entity definitions
│   ├── MyErp.Common/       cross-cutting: results, paging, exceptions, abstractions
│   ├── MyErp.Masters/      business module
│   ├── MyErp.Admin/        business module
│   ├── MyErp.Accounts/     business module
│   └── MyErp.Inventory/    business module
└── frontend/           Angular workspace
    └── src/app/
        ├── core/           guards, interceptors, API services, error handling
        ├── shared/         reusable, feature-agnostic components
        ├── layout/         application shell
        ├── pages/          dashboard and error pages
        └── features/       masters | inventory | accounts | admin (lazy loaded)
```

## Prerequisites

- .NET SDK 9
- Node.js 20.19+ or 22+, and npm
- PostgreSQL 14+

PrimeNG is pinned to v21: v22 introduced a commercial licence check that shows a
banner at runtime without a key. Upgrading the frontend to Angular 22 therefore
means buying a PrimeNG licence or replacing the component library.

## Running

Backend (http://localhost:5235):

```bash
cd backend && dotnet run --project MyErp.Api
```

Frontend (http://localhost:4200):

```bash
cd frontend && npm install && npm start
```

`ng serve` proxies `/api` and `/health` to the backend (see
`frontend/proxy.conf.json`), so the browser stays on one origin and CORS is not
involved during development.

Check the backend is up and can reach the database:

```bash
curl http://localhost:5235/health && curl http://localhost:5235/health/db
```

## Database

One PostgreSQL database behind one shared `AppDbContext`. The connection string
lives in `backend/MyErp.Api/appsettings.json` under
`ConnectionStrings:DefaultConnection` and is never hard-coded in C#.

For local overrides that must not be committed, use user secrets rather than
editing the checked-in file:

```bash
cd backend/MyErp.Api && dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Database=...;Username=...;Password=..."
```

Migrations live in `MyErp.Data` and are driven from the API host:

```bash
cd backend && dotnet ef migrations add <Name> --project MyErp.Data --startup-project MyErp.Api
cd backend && dotnet ef database update --project MyErp.Data --startup-project MyErp.Api
```

Tables and columns are mapped to `snake_case` automatically, because Postgres
folds unquoted identifiers to lower case.

## Architecture rules

These are the constraints the skeleton exists to protect. Breaking one is a
design decision, not a shortcut.

1. **One host, one database, one `DbContext`.** `MyErp.Api` is the only
   executable; `AppDbContext` is the only context.
2. **`MyErp.Api` holds no business code.** No controllers, DTOs, services or
   rules — only startup, middleware, auth and module registration.
3. **Controllers live in their module**, under
   `Features/<Feature>/Controllers/`.
4. **Modules never reference each other.** `Masters`, `Admin`, `Accounts` and
   `Inventory` each reference only `MyErp.Data`, `MyErp.Entities` and
   `MyErp.Common`. If two modules need to share something, it moves to
   `MyErp.Common` or is exposed through the database — not a project reference.
5. **Every module registers itself** in its own `DependencyInjection.cs`. The
   host lists modules; it does not know their internals.
6. **Persistence goes through repositories.** Services depend on
   `IRepository<T>` and `IUnitOfWork`, not on `AppDbContext`.
7. **Business rules are enforced in the backend.** Frontend guards and
   validation are conveniences; the API is the authority.

## Adding a feature

Backend, inside the owning module:

```text
MyErp.Masters/Features/ItemGroup/
├── Controllers/      thin: bind, call a service, return
├── DTOs/             request and response shapes
├── Interfaces/       the service contract
├── Services/         orchestration, using IRepository<T> / IUnitOfWork
└── BusinessRules/    the rules themselves
```

Register the service in the module's `DependencyInjection.cs`. Put the entity in
`MyErp.Entities` and its `IEntityTypeConfiguration<T>` in
`MyErp.Data/Configurations`.

Frontend, in the matching area:

```text
frontend/src/app/features/masters/
├── masters.routes.ts       add the child route here
└── pages/…                 the screens
```

Feature services call `ApiService` from `@core`; failures are normalised and
reported by the HTTP error interceptor, so components handle the success path.

## Version

version-1  base-project
