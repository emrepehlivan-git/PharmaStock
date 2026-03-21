using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products;

public sealed record ProductDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string? Barcode,
    string UnitOfMeasurement,
    string? Category,
    string? Manufacturer,
    string? Brand,
    bool BatchTrackingEnabled,
    bool ExpirationTrackingEnabled,
    bool SerialTrackingEnabled,
    bool ColdChainRequired,
    decimal? MinimumTemperatureCelsius,
    decimal? MaximumTemperatureCelsius,
    decimal? CriticalStockLevel,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastModifiedAt)
{
    public static ProductDto FromEntity(ProductEntity product) =>
        new(
            product.Id,
            product.Code,
            product.Name,
            product.Description,
            product.Barcode,
            product.UnitOfMeasurement,
            product.Category,
            product.Manufacturer,
            product.Brand,
            product.BatchTrackingEnabled,
            product.ExpirationTrackingEnabled,
            product.SerialTrackingEnabled,
            product.ColdChainRequired,
            product.MinimumTemperatureCelsius,
            product.MaximumTemperatureCelsius,
            product.CriticalStockLevel,
            product.IsActive,
            product.CreatedAt,
            product.LastModifiedAt);
}
