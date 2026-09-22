# Accounts features

One folder per feature, each self-contained:

```text
Features/
└── FeatureName/
    ├── Controllers/
    ├── DTOs/
    ├── Interfaces/
    ├── Services/
    └── BusinessRules/
```

Rules:

- Controllers stay thin: bind the request, call a service, shape the response.
- Services depend on `IRepository<T>` / `IUnitOfWork` from `MyErp.Data`, never on
  `AppDbContext` directly.
- Business rules belong in `BusinessRules/`, not in controllers or the frontend.
- Register the feature's services in `../DependencyInjection.cs`.
- Never reference another `MyErp.<Module>` project from here.
