using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProductById;
using PharmaStock.Modules.Product.Domain.Constants;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTests
    : QueryHandlerTestBase<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    protected override IRequestHandler<GetProductByIdQuery, Result<ProductDto>> CreateHandler() =>
        new GetProductByIdQueryHandler(_productRepository);

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFailure()
    {
        var id = Guid.NewGuid();
        var query = GetProductByIdQueryTestData.Query(id);
        _productRepository
            .GetByIdAsync(id, true, Arg.Any<CancellationToken>())
            .Returns((ProductEntity?)null);

        var result = await CreateHandler().Handle(query, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductNotFound);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ReturnsDto()
    {
        var product = GetProductByIdQueryTestData.SampleProduct();
        var query = GetProductByIdQueryTestData.Query(product.Id);
        _productRepository
            .GetByIdAsync(product.Id, true, Arg.Any<CancellationToken>())
            .Returns(product);

        var result = await CreateHandler().Handle(query, Ct);

        var expected = ProductDto.FromEntity(product);
        result.Should().BeSuccessWithValue(expected);
    }
}
