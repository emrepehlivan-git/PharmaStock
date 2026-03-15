using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.DependencyInjection;

namespace PharmaStock.BuildingBlocks.Repositories;

public sealed class EfUnitOfWork<TContext>(TContext dbContext) : IUnitOfWork, IScopedLifetime
    where TContext : DbContext
{
    private readonly TContext _dbContext = Guard.AgainstNull(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}

