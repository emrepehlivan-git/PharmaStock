using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;

namespace PharmaStock.BuildingBlocks.Repositories;

public abstract class EfRepository<TAggregate, TContext> : IRepository<TAggregate>
    where TAggregate : class, IEntity
    where TContext : DbContext
{
    protected readonly TContext DbContext;
    protected readonly DbSet<TAggregate> DbSet;

    public EfRepository(TContext dbContext)
    {
        Guard.AgainstNull(dbContext);
        DbContext = dbContext;
        DbSet = DbContext.Set<TAggregate>();
    }

    public virtual IQueryable<TAggregate> Query(
        bool asNoTracking = true,
        Expression<Func<TAggregate, bool>>? predicate = null)
    {
        IQueryable<TAggregate> query = DbSet;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }

    public virtual async Task<TAggregate?> GetByIdAsync(
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await Query(asNoTracking).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<TAggregate> FindAsync(
        Expression<Func<TAggregate, bool>> predicate,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await Query(asNoTracking, predicate).FirstOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException($"Entity of type {typeof(TAggregate).Name} with id {predicate} not found.");
    }

    public virtual async Task<PagedResult<TAggregate>> GetPageAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TAggregate, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        Guard.Positive(pageNumber);
        Guard.Positive(pageSize);

        IQueryable<TAggregate> query = Query(asNoTracking, predicate);

        return await query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);
    }

    public virtual async Task<TAggregate> AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        Guard.AgainstNull(entity);

        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        Guard.AgainstNull(entity);

        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(TAggregate entity, CancellationToken cancellationToken = default)
    {
        Guard.AgainstNull(entity);

        DbSet.Remove(entity);
        return Task.CompletedTask;
    }
}

