using System.Linq.Expressions;

namespace PharmaStock.BuildingBlocks.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }

    IQueryable<T> ApplyIncludes(IQueryable<T> query);
}
