using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);
        if (product is null)
            return Result.Failure(ProductConstants.Messages.ProductNotFound);

        bool codeExists = await productRepository.ExistsByCodeAsync(request.Code, request.Id, cancellationToken);
        if (codeExists)
            return Result.Failure(ProductConstants.Messages.ProductCodeAlreadyExists);

        product.Update(
            code: request.Code,
            name: request.Name,
            description: request.Description,
            barcode: request.Barcode,
            unitOfMeasurement: request.UnitOfMeasurement,
            category: request.Category,
            manufacturer: request.Manufacturer,
            brand: request.Brand,
            batchTrackingEnabled: request.BatchTrackingEnabled,
            expirationTrackingEnabled: request.ExpirationTrackingEnabled,
            serialTrackingEnabled: request.SerialTrackingEnabled,
            coldChainRequired: request.ColdChainRequired,
            minimumTemperatureCelsius: request.MinimumTemperatureCelsius,
            maximumTemperatureCelsius: request.MaximumTemperatureCelsius,
            criticalStockLevel: request.CriticalStockLevel);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
