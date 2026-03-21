using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;
using PharmaStock.Modules.Product.Presentation.Dtos.Products.UpdateProduct;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class UpdateProductEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/products/{id:guid}", HandleUpdateProductAsync)
            .WithName("UpdateProduct")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleUpdateProductAsync(
        Guid id,
        UpdateProductRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        UpdateProductCommand command = new(
            Id: id,
            Code: request.Code,
            Name: request.Name,
            Description: request.Description,
            Barcode: request.Barcode,
            UnitOfMeasurement: request.UnitOfMeasurement,
            Category: request.Category,
            Manufacturer: request.Manufacturer,
            Brand: request.Brand,
            BatchTrackingEnabled: request.BatchTrackingEnabled,
            ExpirationTrackingEnabled: request.ExpirationTrackingEnabled,
            SerialTrackingEnabled: request.SerialTrackingEnabled,
            ColdChainRequired: request.ColdChainRequired,
            MinimumTemperatureCelsius: request.MinimumTemperatureCelsius,
            MaximumTemperatureCelsius: request.MaximumTemperatureCelsius,
            CriticalStockLevel: request.CriticalStockLevel,
            Reason: request.Reason);

        Result result = await mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Results.NoContent();

        if (result.Error == ProductErrors.NotFound)
            return Results.NotFound(new { error = result.Error });

        return Results.BadRequest(new { error = result.Error });
    }
}
