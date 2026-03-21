using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

public sealed class SetProductActiveCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SetProductActiveCommand, Result>
{
    public async ValueTask<Result> Handle(SetProductActiveCommand request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);
        if (product is null)
            return Result.Failure(ProductConstants.Messages.ProductNotFound);

        product.SetActive(request.IsActive);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
