using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProducts;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class GetProductsEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products", HandleGetProductsAsync)
            .WithName("GetProducts")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleGetProductsAsync(
        IMediator mediator,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 20,
        string? search = null,
        string? category = null,
        bool? isActive = null,
        ProductSortField sortBy = ProductSortField.Code,
        bool sortDescending = false)
    {
        var query = new GetProductsQuery(
            PageNumber: page,
            PageSize: pageSize,
            Search: search,
            Category: category,
            IsActive: isActive,
            SortBy: sortBy,
            SortDescending: sortDescending);

        Result<PagedResult<ProductListItemDto>> result = await mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(new { error = result.Error });
    }
}
