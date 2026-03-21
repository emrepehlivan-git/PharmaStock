using Mediator;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products.Audit;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

public sealed class SetProductActiveCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IComplianceAuditLogWriter complianceAuditLogWriter,
    IAuditUserAccessor auditUserAccessor) : IRequestHandler<SetProductActiveCommand, Result>
{
    public async ValueTask<Result> Handle(SetProductActiveCommand request, CancellationToken cancellationToken)
    {
        ProductEntity? product = await productRepository.GetByIdAsync(request.Id, asNoTracking: false, cancellationToken);
        if (product is null)
            return Result.Failure(ProductConstants.Messages.ProductNotFound);

        string previousValue = ProductAuditSnapshotText.SerializeActivation(product.IsActive);

        product.SetActive(request.IsActive);

        string newValue = ProductAuditSnapshotText.SerializeActivation(product.IsActive);

        DateTime occurredAtUtc = DateTime.UtcNow;
        var auditEntry = ComplianceAuditLogEntry.Create(
            ProductConstants.Compliance.AggregateType,
            product.Id,
            ProductConstants.Compliance.OperationType.ActivationChanged,
            previousValue: previousValue,
            newValue: newValue,
            reason: request.Reason,
            performedByUserId: auditUserAccessor.UserId,
            occurredAtUtc: occurredAtUtc);
        await complianceAuditLogWriter.WriteAsync(auditEntry, cancellationToken);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
