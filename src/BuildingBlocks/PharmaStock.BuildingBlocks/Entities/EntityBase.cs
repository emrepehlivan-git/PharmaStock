namespace PharmaStock.BuildingBlocks.Entities;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; protected init; }

    protected EntityBase()
    {
        Id = Guid.CreateVersion7();
    }
}
