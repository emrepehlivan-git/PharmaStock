using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products;

public sealed record ProductListItemDto(
    Guid Id,
    string Code,
    string Name,
    string? Category,
    string UnitOfMeasurement,
    bool IsActive,
    DateTime CreatedAt)
{
    public static ProductListItemDto FromEntity(ProductEntity product) =>
        new(
            product.Id,
            product.Code,
            product.Name,
            product.Category,
            product.UnitOfMeasurement,
            product.IsActive,
            product.CreatedAt);
}
