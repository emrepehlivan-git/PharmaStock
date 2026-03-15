using System.Linq.Expressions;

namespace PharmaStock.BuildingBlocks.Specifications;

public sealed record OrderByClause<T>(Expression<Func<T, object?>> KeySelector, bool Descending);

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }

    IQueryable<T> ApplyIncludes(IQueryable<T> query);

    IReadOnlyList<OrderByClause<T>> OrderByClauses { get; }

    bool UseSplitQuery { get; }
}
