using System.Linq.Expressions;
using PharmaStock.BuildingBlocks.Specifications;
using PharmaStock.Modules.Product.Application.Products;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Specifications;

public sealed class ProductListSpec : SpecificationBase<ProductEntity>
{
    private readonly bool _applySearch;
    private readonly string _searchTermLower;
    private readonly bool _applyCategory;
    private readonly string _categoryTrimmed;
    private readonly bool _applyIsActive;
    private readonly bool _isActiveValue;
    private readonly ProductSortField _sortBy;
    private readonly bool _sortDescending;

    public ProductListSpec(
        string? search,
        string? category,
        bool? isActive,
        ProductSortField sortBy,
        bool sortDescending)
    {
        _applySearch = !string.IsNullOrWhiteSpace(search);
        _searchTermLower = search?.Trim().ToLowerInvariant() ?? string.Empty;
        _applyCategory = !string.IsNullOrWhiteSpace(category);
        _categoryTrimmed = category?.Trim() ?? string.Empty;
        _applyIsActive = isActive.HasValue;
        _isActiveValue = isActive ?? false;
        _sortBy = sortBy;
        _sortDescending = sortDescending;
    }

    public override Expression<Func<ProductEntity, bool>> Criteria => p =>
        (!_applySearch ||
            p.Code.ToLower().Contains(_searchTermLower) ||
            p.Name.ToLower().Contains(_searchTermLower))
        && (!_applyCategory || p.Category == _categoryTrimmed)
        && (!_applyIsActive || p.IsActive == _isActiveValue);

    public override IReadOnlyList<OrderByClause<ProductEntity>> OrderByClauses =>
        _sortBy switch
        {
            ProductSortField.Name => [OrderBy(p => p.Name, _sortDescending)],
            ProductSortField.CreatedAt => [OrderBy(p => p.CreatedAt, _sortDescending)],
            _ => [OrderBy(p => p.Code, _sortDescending)]
        };
}
