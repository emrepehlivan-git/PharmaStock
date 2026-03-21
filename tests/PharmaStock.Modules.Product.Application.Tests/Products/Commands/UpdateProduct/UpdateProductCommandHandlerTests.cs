using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;
using PharmaStock.Modules.Product.Domain.Constants;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandlerTests
    : CommandHandlerTestBase<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    protected override IRequestHandler<UpdateProductCommand, Result> CreateHandler() =>
        new UpdateProductCommandHandler(_productRepository, _unitOfWork);

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().Build();
        _productRepository
            .GetByIdAsync(cmd.Id, false, Arg.Any<CancellationToken>())
            .Returns((ProductEntity?)null);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductNotFound);
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<ProductEntity>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenCodeBelongsToAnotherProduct_ReturnsFailure()
    {
        var product = UpdateProductCommandTestData.ExistingProduct();
        var cmd = UpdateProductCommandTestData.New()
            .WithId(product.Id)
            .WithCode("OTHER-001")
            .Build();
        _productRepository
            .GetByIdAsync(cmd.Id, false, Arg.Any<CancellationToken>())
            .Returns(product);
        _productRepository
            .ExistsByCodeAsync(cmd.Code, cmd.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductCodeAlreadyExists);
        await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<ProductEntity>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenValid_UpdatesProductAndSaves()
    {
        var product = UpdateProductCommandTestData.ExistingProduct();
        var cmd = UpdateProductCommandTestData.New()
            .WithId(product.Id)
            .WithName("Güncel ad")
            .Build();
        _productRepository
            .GetByIdAsync(product.Id, false, Arg.Any<CancellationToken>())
            .Returns(product);
        _productRepository
            .ExistsByCodeAsync(cmd.Code, product.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await CreateHandler().Handle(cmd, Ct);

        result.Should().BeSuccess();
        await _productRepository.Received(1).UpdateAsync(
            Arg.Is<ProductEntity>(p => p.Id == product.Id && p.Name == "Güncel ad"),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
