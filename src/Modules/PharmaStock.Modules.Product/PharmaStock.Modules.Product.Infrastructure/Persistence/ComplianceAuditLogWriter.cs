using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.DependencyInjection;
using PharmaStock.BuildingBlocks.Entities;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence;

public sealed class ComplianceAuditLogWriter(ProductDbContext context)
    : IComplianceAuditLogWriter, IScopedLifetime
{
    public async Task WriteAsync(ComplianceAuditLogEntry entry, CancellationToken cancellationToken = default)
    {
        await context.Set<ComplianceAuditLogEntry>().AddAsync(entry, cancellationToken);
    }
}
