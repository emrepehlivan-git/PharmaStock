using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.BuildingBlocks.Repositories;

public sealed class EfUnitOfWork<TContext>(TContext dbContext) : IUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _dbContext = Guard.AgainstNull(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
}

