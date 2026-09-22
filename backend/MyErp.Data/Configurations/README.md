# Entity configurations

`IEntityTypeConfiguration<T>` classes for the shared `AppDbContext` live here.
They are applied automatically by `AppDbContext.OnModelCreating` via
`ApplyConfigurationsFromAssembly`.

Naming: one file per entity, `<Entity>Configuration.cs`.
