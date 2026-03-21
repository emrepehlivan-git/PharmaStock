using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;

public sealed class GetProductByCodeQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByCodeQuery, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(GetProductByCodeQuery request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByCodeAsync(request.Code, asNoTracking: true, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure(ProductConstants.Messages.ProductNotFound);

        return Result<ProductDto>.Success(ProductDto.FromEntity(product));
    }
}
