using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Presentation.Dtos.Products.CreateProduct;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class CreateProductEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/products", HandleCreateProductAsync)
            .WithName("CreateProduct")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleCreateProductAsync(
        CreateProductRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        CreateProductCommand command = new(
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

        Result<Guid> result = await mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            Guid id = result.Value!;
            return Results.Created($"/api/products/{id}", new { id });
        }

        return Results.BadRequest(new { error = result.Error });
    }
}
