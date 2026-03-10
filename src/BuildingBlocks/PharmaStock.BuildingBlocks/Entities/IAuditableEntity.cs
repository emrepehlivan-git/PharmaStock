namespace PharmaStock.BuildingBlocks.Entities;

public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedAt { get; }
    string? LastModifiedBy { get; }
}
