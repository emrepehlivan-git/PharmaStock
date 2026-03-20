using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Presentation.Dtos.Products.CreateProduct;

namespace PharmaStock.Modules.Product.Presentation;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/products", HandleCreateProductAsync)
            .WithName("CreateProduct")
            .WithTags("Products");

        return endpoints;
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
            CriticalStockLevel: request.CriticalStockLevel);

        Result<Guid> result = await mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            Guid id = result.Value!;
            return Results.Created($"/api/products/{id}", new { id });
        }

        return Results.BadRequest(new { error = result.Error });
    }
}
