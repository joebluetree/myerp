using Microsoft.EntityFrameworkCore;
using MyErp.Common.Interfaces;
using MyErp.Entities.Abstractions;

namespace MyErp.Data.Context;

/// <summary>
/// The single application DbContext. Every module persists through this context;
/// modules contribute their mappings as <see cref="IEntityTypeConfiguration{TEntity}"/>
/// classes, which are picked up from the configuration assemblies below.
/// </summary>
public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ICurrentUser? currentUser = null,
    IDateTimeProvider? dateTimeProvider = null) : DbContext(options)
{
    private readonly IDateTimeProvider _clock = dateTimeProvider ?? new Common.Time.SystemDateTimeProvider();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mappings live next to the code that owns them. Configuration classes are
        // discovered from this assembly today; register further assemblies here as
        // modules start mapping their own entities.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.ApplySnakeCaseNames();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }

    private void ApplyAuditInformation()
    {
        var now = _clock.UtcNow;
        var user = currentUser?.UserId;

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.CreatedBy = user;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAtUtc = now;
                    entry.Entity.ModifiedBy = user;
                    break;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State != EntityState.Deleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAtUtc = now;
            entry.Entity.DeletedBy = user;
        }
    }
}
