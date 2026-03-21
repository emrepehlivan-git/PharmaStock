using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;
using PharmaStock.Modules.Product.Domain.Constants;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.SetProductActive;

public sealed class SetProductActiveCommandHandlerTests
    : CommandHandlerTestBase<SetProductActiveCommand, Result>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    protected override IRequestHandler<SetProductActiveCommand, Result> CreateHandler() =>
        new SetProductActiveCommandHandler(_productRepository, _unitOfWork);

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFailure()
    {
        var id = Guid.NewGuid();
        var cmd = SetProductActiveCommandTestData.With(id, isActive: true);
        _productRepository
            .GetByIdAsync(id, false, Arg.Any<CancellationToken>())
            .Returns((ProductEntity?)null);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductNotFound);
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<ProductEntity>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenProductExists_SetsActiveAndSaves(bool isActive)
    {
        var product = SetProductActiveCommandTestData.ExistingProduct();
        var cmd = SetProductActiveCommandTestData.With(product.Id, isActive);
        _productRepository
            .GetByIdAsync(product.Id, false, Arg.Any<CancellationToken>())
            .Returns(product);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeSuccess();
        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<ProductEntity>(p => p.Id == product.Id && p.IsActive == isActive),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
