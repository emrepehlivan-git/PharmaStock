using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.DependencyInjection;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence;

public sealed class ProductRepository(ProductDbContext context)
    : EfRepository<ProductEntity, ProductDbContext>(context), IProductRepository, IScopedLifetime
{
    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId, CancellationToken cancellationToken = default)
    {
        string normalizedCode = ProductCodeNormalizer.Normalize(code);
        var query = DbSet.AsNoTracking().Where(p => p.Code == normalizedCode);
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<ProductEntity?> GetByCodeAsync(string code, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        string normalizedCode = ProductCodeNormalizer.Normalize(code);
        return await Query(asNoTracking, p => p.Code == normalizedCode).FirstOrDefaultAsync(cancellationToken);
    }
}
