using Mediator;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    string? Category,
    bool? IsActive,
    ProductSortField SortBy,
    bool SortDescending) : IRequest<Result<PagedResult<ProductListItemDto>>>;
