namespace PharmaStock.BuildingBlocks.Entities;

public abstract class AuditableSoftDeletableEntityBase : AuditableEntityBase, ISoftDeletableEntity
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }
}
