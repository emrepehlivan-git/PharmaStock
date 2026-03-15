using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence;

public sealed class ProductRepository(ProductDbContext context)
    : EfRepository<ProductEntity, ProductDbContext>(context), IProductRepository
{
    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId, CancellationToken cancellationToken = default)
    {
        Guard.AgainstNullOrWhiteSpace(code);
        var query = DbSet.AsNoTracking().Where(p => p.Code == code);
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        return await query.AnyAsync(cancellationToken);
    }
}
