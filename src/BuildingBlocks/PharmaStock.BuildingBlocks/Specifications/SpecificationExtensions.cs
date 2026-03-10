using System.Linq;

namespace PharmaStock.BuildingBlocks.Specifications;

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification)
    {
        query = specification.ApplyIncludes(query);
        return query.Where(specification.Criteria);
    }
}
