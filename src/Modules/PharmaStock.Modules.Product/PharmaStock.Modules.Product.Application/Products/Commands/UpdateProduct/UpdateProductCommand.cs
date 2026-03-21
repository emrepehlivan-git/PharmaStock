using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Validation;

namespace PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
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
    decimal? CriticalStockLevel) : IRequest<Result>, IValidatableRequest;
