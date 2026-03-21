using FluentAssertions;
using Mediator;
using NSubstitute;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Specifications;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProducts;
using PharmaStock.Tests.Common.Assertions;
using PharmaStock.Tests.Common.Base;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProducts;

public sealed class GetProductsQueryHandlerTests
    : QueryHandlerTestBase<GetProductsQuery, Result<PagedResult<ProductListItemDto>>>
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();

    protected override IRequestHandler<GetProductsQuery, Result<PagedResult<ProductListItemDto>>> CreateHandler() =>
        new GetProductsQueryHandler(_productRepository);

    [Fact]
    public async Task Handle_WhenRepositoryReturnsProducts_ReturnsPagedDtos()
    {
        var query = GetProductsQueryTestData.Default();
        var product = GetProductsQueryTestData.SampleProduct();
        var page = new PagedResult<ProductEntity>([product], totalCount: 1, pageNumber: query.PageNumber, pageSize: query.PageSize);
        _productRepository
            .GetPageAsync(query.PageNumber, query.PageSize, Arg.Any<ISpecification<ProductEntity>>(), true, Arg.Any<CancellationToken>())
            .Returns(page);

        var result = await CreateHandler().Handle(query, Ct);

        result.Should().BeSuccess();
        result.Value!.TotalCount.Should().Be(1);
        result.Value.PageNumber.Should().Be(query.PageNumber);
        result.Value.PageSize.Should().Be(query.PageSize);
        result.Value.Items.Should().ContainSingle().Which.Should().Be(ProductListItemDto.FromEntity(product));
        await _productRepository.Received(1).GetPageAsync(
            query.PageNumber,
            query.PageSize,
            Arg.Any<ISpecification<ProductEntity>>(),
            true,
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsNoRows_ReturnsEmptyPage()
    {
        var query = new GetProductsQuery(
            PageNumber: 2,
            PageSize: 5,
            Search: "ara",
            Category: "Kat",
            IsActive: true,
            SortBy: ProductSortField.Name,
            SortDescending: true);
        var page = new PagedResult<ProductEntity>([], totalCount: 0, pageNumber: query.PageNumber, pageSize: query.PageSize);
        _productRepository
            .GetPageAsync(query.PageNumber, query.PageSize, Arg.Any<ISpecification<ProductEntity>>(), true, Arg.Any<CancellationToken>())
            .Returns(page);

        var result = await CreateHandler().Handle(query, Ct);

        result.Should().BeSuccess();
        result.Value!.Items.Should().BeEmpty();
        result.Value.TotalCount.Should().Be(0);
        result.Value.PageNumber.Should().Be(2);
        result.Value.PageSize.Should().Be(5);
    }
}
