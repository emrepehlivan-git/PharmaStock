using System.Text.Json;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Audit;

public static class ProductAuditSnapshotText
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static string Serialize(ProductAuditSnapshot snapshot) =>
        JsonSerializer.Serialize(snapshot, Options);

    public static string SerializeFromEntity(ProductEntity product) =>
        Serialize(ProductAuditSnapshot.FromEntity(product));

    public static string SerializeActivation(bool isActive) =>
        JsonSerializer.Serialize(new { isActive }, Options);
}
