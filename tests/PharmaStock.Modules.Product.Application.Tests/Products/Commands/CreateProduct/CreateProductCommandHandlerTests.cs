using FluentAssertions;
using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Domain.Constants;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;

using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandlerTests
    : CommandHandlerTestBase<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IComplianceAuditLogWriter _complianceAuditLogWriter = Substitute.For<IComplianceAuditLogWriter>();
    private readonly IAuditUserAccessor _auditUserAccessor = Substitute.For<IAuditUserAccessor>();

    protected override IRequestHandler<CreateProductCommand, Result<Guid>> CreateHandler() =>
        new CreateProductCommandHandler(_productRepository, _unitOfWork, _complianceAuditLogWriter, _auditUserAccessor);

    [Fact]
    public async Task Handle_WhenCodeAlreadyExists_ReturnsFailure()
    {
        var cmd = CreateProductCommandTestData.Valid();
        _productRepository.ExistsByCodeAsync(cmd.Code, null, Arg.Any<CancellationToken>()).Returns(true);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductCodeAlreadyExists);
        await _productRepository.DidNotReceive().AddAsync(Arg.Any<ProductEntity>(), Arg.Any<CancellationToken>());
        await _complianceAuditLogWriter.DidNotReceive()
            .WriteAsync(Arg.Any<ComplianceAuditLogEntry>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCodeIsUnique_AddsProductAndReturnsId()
    {
        var cmd = CreateProductCommandTestData.Valid();
        _productRepository.ExistsByCodeAsync(cmd.Code, null, Arg.Any<CancellationToken>()).Returns(false);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeSuccess();
        result.Value.Should().NotBeEmpty();
        await _productRepository.Received(1).AddAsync(
            Arg.Is<ProductEntity>(p =>
                p.Code == cmd.Code
                && p.Name == cmd.Name
                && p.UnitOfMeasurement == cmd.UnitOfMeasurement),
            Arg.Any<CancellationToken>());
        await _complianceAuditLogWriter.Received(1).WriteAsync(
            Arg.Is<ComplianceAuditLogEntry>(e =>
                e.OperationType == ProductConstants.Compliance.OperationType.Created
                && e.AggregateType == ProductConstants.Compliance.AggregateType),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
