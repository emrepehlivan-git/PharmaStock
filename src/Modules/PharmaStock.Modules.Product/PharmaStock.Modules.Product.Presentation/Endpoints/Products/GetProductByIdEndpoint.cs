using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProductById;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class GetProductByIdEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products/{id:guid}", HandleGetProductByIdAsync)
            .WithName("GetProductById")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleGetProductByIdAsync(
        Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        Result<ProductDto> result = await mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        if (result.Error == ProductErrors.NotFound)
            return Results.NotFound(new { error = result.Error });

        return Results.BadRequest(new { error = result.Error });
    }
}
