using PharmaStock.BuildingBlocks.Repositories;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Products;

public interface IProductRepository : IRepository<ProductEntity>
{
    Task<bool> ExistsByCodeAsync(string code, Guid? excludeId, CancellationToken cancellationToken = default);
}
