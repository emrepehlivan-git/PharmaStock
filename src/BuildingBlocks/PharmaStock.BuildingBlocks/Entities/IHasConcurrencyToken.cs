namespace PharmaStock.BuildingBlocks.Entities;

public interface IHasConcurrencyToken : IEntity
{
    byte[] RowVersion { get; }
}
