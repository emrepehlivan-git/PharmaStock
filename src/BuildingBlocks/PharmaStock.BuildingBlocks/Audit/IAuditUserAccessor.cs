namespace PharmaStock.BuildingBlocks.Audit;

public interface IAuditUserAccessor
{
    string? UserId { get; }
}
