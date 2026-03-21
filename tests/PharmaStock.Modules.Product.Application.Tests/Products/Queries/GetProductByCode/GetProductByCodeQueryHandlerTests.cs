using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;
using PharmaStock.Modules.Product.Domain.Constants;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProductByCode;

public sealed class GetProductByCodeQueryHandlerTests
    : QueryHandlerTestBase<GetProductByCodeQuery, Result<ProductDto>>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    protected override IRequestHandler<GetProductByCodeQuery, Result<ProductDto>> CreateHandler() =>
        new GetProductByCodeQueryHandler(_productRepository);

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFailure()
    {
        var query = GetProductByCodeQueryTestData.Query("YOK-001");
        _productRepository
            .GetByCodeAsync(query.Code, true, Arg.Any<CancellationToken>())
            .Returns((ProductEntity?)null);

        var result = await CreateHandler().Handle(query, Ct);

        result.Should().BeFailure(ProductConstants.Messages.ProductNotFound);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ReturnsDto()
    {
        var product = GetProductByCodeQueryTestData.SampleProduct();
        var query = GetProductByCodeQueryTestData.Query(product.Code);
        _productRepository
            .GetByCodeAsync(query.Code, true, Arg.Any<CancellationToken>())
            .Returns(product);

        var result = await CreateHandler().Handle(query, Ct);

        var expected = ProductDto.FromEntity(product);
        result.Should().BeSuccessWithValue(expected);
    }
}
