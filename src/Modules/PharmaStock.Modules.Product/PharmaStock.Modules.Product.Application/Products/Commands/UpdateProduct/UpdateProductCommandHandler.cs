using Mediator;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products.Audit;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IComplianceAuditLogWriter complianceAuditLogWriter,
    IAuditUserAccessor auditUserAccessor) : IRequestHandler<UpdateProductCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);
        if (product is null)
            return Result.Failure(ProductConstants.Messages.ProductNotFound);

        bool codeExists = await productRepository.ExistsByCodeAsync(request.Code, request.Id, cancellationToken);
        if (codeExists)
            return Result.Failure(ProductConstants.Messages.ProductCodeAlreadyExists);

        string previousSnapshot = ProductAuditSnapshotText.SerializeFromEntity(product);

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

        string newSnapshot = ProductAuditSnapshotText.SerializeFromEntity(product);

        DateTime occurredAtUtc = DateTime.UtcNow;
        var auditEntry = ComplianceAuditLogEntry.Create(
            ProductConstants.Compliance.AggregateType,
            product.Id,
            ProductConstants.Compliance.OperationType.Updated,
            previousValue: previousSnapshot,
            newValue: newSnapshot,
            reason: request.Reason,
            performedByUserId: auditUserAccessor.UserId,
            occurredAtUtc: occurredAtUtc);
        await complianceAuditLogWriter.WriteAsync(auditEntry, cancellationToken);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
