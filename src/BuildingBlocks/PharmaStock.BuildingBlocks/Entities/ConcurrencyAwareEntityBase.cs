namespace PharmaStock.BuildingBlocks.Entities;

public abstract class ConcurrencyAwareEntityBase : AuditableSoftDeletableEntityBase, IHasConcurrencyToken
{
    public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();
}
