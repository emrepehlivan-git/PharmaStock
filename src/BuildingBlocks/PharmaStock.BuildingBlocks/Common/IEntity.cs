namespace PharmaStock.BuildingBlocks.Common;

/// <summary>
/// All aggregate root and entity types must implement this interface.
/// Ids are generated as Guid v7.
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}

