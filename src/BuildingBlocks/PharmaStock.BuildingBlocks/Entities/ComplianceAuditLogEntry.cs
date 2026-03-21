namespace PharmaStock.BuildingBlocks.Entities;

public sealed class ComplianceAuditLogEntry : EntityBase
{
    public string AggregateType { get; private set; } = string.Empty;
    public Guid AggregateId { get; private set; }
    public string OperationType { get; private set; } = string.Empty;
    public string? PreviousValue { get; private set; }
    public string? NewValue { get; private set; }
    public string? Reason { get; private set; }
    public string? PerformedByUserId { get; private set; }
    public DateTime OccurredAtUtc { get; private set; }

    private ComplianceAuditLogEntry()
    {
    }

    public static ComplianceAuditLogEntry Create(
        string aggregateType,
        Guid aggregateId,
        string operationType,
        string? previousValue,
        string? newValue,
        string? reason,
        string? performedByUserId,
        DateTime occurredAtUtc)
    {
        return new ComplianceAuditLogEntry
        {
            AggregateType = aggregateType,
            AggregateId = aggregateId,
            OperationType = operationType,
            PreviousValue = previousValue,
            NewValue = newValue,
            Reason = reason,
            PerformedByUserId = performedByUserId,
            OccurredAtUtc = occurredAtUtc
        };
    }
}
