using Mediator;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products.Audit;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IComplianceAuditLogWriter complianceAuditLogWriter,
    IAuditUserAccessor auditUserAccessor) : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async ValueTask<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool codeExists = await productRepository.ExistsByCodeAsync(request.Code, null, cancellationToken);
        if (codeExists)
            return Result<Guid>.Failure(ProductConstants.Messages.ProductCodeAlreadyExists);

        var product = ProductEntity.Create(
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

        await productRepository.AddAsync(product, cancellationToken);

        DateTime occurredAtUtc = DateTime.UtcNow;
        var auditEntry = ComplianceAuditLogEntry.Create(
            ProductConstants.Compliance.AggregateType,
            product.Id,
            ProductConstants.Compliance.OperationType.Created,
            previousValue: null,
            newValue: ProductAuditSnapshotText.SerializeFromEntity(product),
            reason: request.Reason,
            performedByUserId: auditUserAccessor.UserId,
            occurredAtUtc: occurredAtUtc);
        await complianceAuditLogWriter.WriteAsync(auditEntry, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}
