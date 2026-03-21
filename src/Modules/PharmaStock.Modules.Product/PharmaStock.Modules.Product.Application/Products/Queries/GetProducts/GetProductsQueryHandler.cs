using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products.Specifications;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, Result<PagedResult<ProductListItemDto>>>
{
    public async ValueTask<Result<PagedResult<ProductListItemDto>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ProductListSpec(
            request.Search,
            request.Category,
            request.IsActive,
            request.SortBy,
            request.SortDescending);

        PagedResult<ProductEntity> page = await productRepository.GetPageAsync(
            request.PageNumber,
            request.PageSize,
            spec,
            asNoTracking: true,
            cancellationToken);

        var items = page.Items.Select(ProductListItemDto.FromEntity).ToList();

        return Result<PagedResult<ProductListItemDto>>.Success(
            new PagedResult<ProductListItemDto>(items, page.TotalCount, page.PageNumber, page.PageSize));
    }
}
