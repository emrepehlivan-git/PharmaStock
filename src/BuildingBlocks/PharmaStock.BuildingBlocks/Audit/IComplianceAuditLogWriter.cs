using PharmaStock.BuildingBlocks.Entities;

namespace PharmaStock.BuildingBlocks.Audit;

public interface IComplianceAuditLogWriter
{
    Task WriteAsync(ComplianceAuditLogEntry entry, CancellationToken cancellationToken = default);
}
