namespace PharmaStock.Modules.Product.Presentation.Dtos.Products.CreateProduct;

public sealed record CreateProductRequest(
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
    string? Reason);

