using System.Linq.Expressions;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.BuildingBlocks.Repositories;

public interface IRepository<TAggregate>
    where TAggregate : class, IEntity
{
    IQueryable<TAggregate> Query(
        bool asNoTracking = true,
        Expression<Func<TAggregate, bool>>? predicate = null);

    Task<TAggregate?> GetByIdAsync(
        Guid id,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<TAggregate> FindAsync(
        Expression<Func<TAggregate, bool>> predicate,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TAggregate>> GetPageAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TAggregate, bool>>? predicate = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<TAggregate> AddAsync(TAggregate entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TAggregate entity, CancellationToken cancellationToken = default);
}

