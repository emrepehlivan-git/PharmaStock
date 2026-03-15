using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PharmaStock.BuildingBlocks.Specifications;

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification)
        where T : class
    {
        if (specification.UseSplitQuery)
            query = query.AsSplitQuery();

        query = specification.ApplyIncludes(query);
        query = query.Where(specification.Criteria);

        var orderByClauses = specification.OrderByClauses;
        if (orderByClauses.Count > 0)
        {
            var first = orderByClauses[0];
            IOrderedQueryable<T> ordered = first.Descending
                ? query.OrderByDescending(first.KeySelector)
                : query.OrderBy(first.KeySelector);

            for (var i = 1; i < orderByClauses.Count; i++)
            {
                var clause = orderByClauses[i];
                ordered = clause.Descending
                    ? ordered.ThenByDescending(clause.KeySelector)
                    : ordered.ThenBy(clause.KeySelector);
            }
            query = ordered;
        }

        return query;
    }
}
