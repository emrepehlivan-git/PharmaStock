using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class GetProductByCodeEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products/by-code/{code}", HandleGetProductByCodeAsync)
            .WithName("GetProductByCode")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleGetProductByCodeAsync(
        string code,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByCodeQuery(code);
        Result<ProductDto> result = await mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        if (result.Error == ProductErrors.NotFound)
            return Results.NotFound(new { error = result.Error });

        return Results.BadRequest(new { error = result.Error });
    }
}
