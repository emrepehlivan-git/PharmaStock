using Microsoft.EntityFrameworkCore;

namespace PharmaStock.BuildingBlocks.Common;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        Guard.Positive(pageNumber);
        Guard.Positive(pageSize);

        var totalCount = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }

    public static PagedResult<T> ToPagedResult<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize)
    {
        Guard.Positive(pageNumber);
        Guard.Positive(pageSize);

        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = source.Count();

        return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
    }
}

