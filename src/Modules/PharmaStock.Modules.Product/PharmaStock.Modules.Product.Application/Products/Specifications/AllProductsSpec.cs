using System.Linq.Expressions;
using PharmaStock.BuildingBlocks.Specifications;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products.Specifications;

public sealed class AllProductsSpec : SpecificationBase<ProductEntity>
{
    public override Expression<Func<ProductEntity, bool>> Criteria => _ => true;
}
