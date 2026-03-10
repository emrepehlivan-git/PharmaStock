namespace PharmaStock.BuildingBlocks.Common;

public sealed class PagedResult<T>(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IReadOnlyList<T> Items { get; } = Guard.AgainstNull(items);
    public int TotalCount { get; } = Guard.AgainstNegative(totalCount);
    public int PageNumber { get; } = Guard.Positive(pageNumber);
    public int PageSize { get; } = Guard.Positive(pageSize);
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

