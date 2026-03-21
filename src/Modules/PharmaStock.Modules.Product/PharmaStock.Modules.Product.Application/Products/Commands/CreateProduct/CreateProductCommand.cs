using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Validation;

namespace PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
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
    string? Reason) : IRequest<Result<Guid>>, IValidatableRequest;
