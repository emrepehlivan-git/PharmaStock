namespace PharmaStock.BuildingBlocks.Entities;

public abstract class AuditableEntityBase : EntityBase, IAuditableEntity
{
    public DateTime CreatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? LastModifiedAt { get; protected set; }
    public string? LastModifiedBy { get; protected set; }
}
