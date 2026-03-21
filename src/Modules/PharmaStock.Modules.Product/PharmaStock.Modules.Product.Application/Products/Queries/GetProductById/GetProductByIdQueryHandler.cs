using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByIdAsync(request.Id, asNoTracking: true, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure(ProductConstants.Messages.ProductNotFound);

        return Result<ProductDto>.Success(ProductDto.FromEntity(product));
    }
}
