namespace PharmaStock.BuildingBlocks.Entities;

public interface ISoftDeletableEntity : IEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
}
