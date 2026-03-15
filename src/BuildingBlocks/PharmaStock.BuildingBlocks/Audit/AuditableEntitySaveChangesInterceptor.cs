using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.DependencyInjection;

namespace PharmaStock.BuildingBlocks.Audit;

public sealed class AuditableEntitySaveChangesInterceptor(IAuditUserAccessor auditUserAccessor) : SaveChangesInterceptor, IScopedLifetime
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditFields(DbContext? context)
    {
        if (context is null)
            return;

        var userId = auditUserAccessor.UserId;
        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = utcNow;
                entry.Property(nameof(IAuditableEntity.CreatedBy)).CurrentValue = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditableEntity.LastModifiedAt)).CurrentValue = utcNow;
                entry.Property(nameof(IAuditableEntity.LastModifiedBy)).CurrentValue = userId;
            }
        }
    }
}
