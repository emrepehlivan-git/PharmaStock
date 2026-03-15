using System.Linq.Expressions;

namespace PharmaStock.BuildingBlocks.Specifications;

public abstract class SpecificationBase<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> Criteria { get; }

    public virtual IQueryable<T> ApplyIncludes(IQueryable<T> query) => query;

    public virtual IReadOnlyList<OrderByClause<T>> OrderByClauses => [];

    public virtual bool UseSplitQuery => false;

    protected static OrderByClause<T> OrderBy(Expression<Func<T, object?>> keySelector, bool descending = false) =>
        new(keySelector, descending);

    public ISpecification<T> And(ISpecification<T> other) =>
        new CombinedSpecification<T>(this, other, ExpressionCombiner.And);

    public ISpecification<T> Or(ISpecification<T> other) =>
        new CombinedSpecification<T>(this, other, ExpressionCombiner.Or);

    public ISpecification<T> Not() => new NegatedSpecification<T>(this);
}

internal sealed class CombinedSpecification<T>(
    ISpecification<T> left,
    ISpecification<T> right,
    Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> combiner) : SpecificationBase<T>
{
    public override Expression<Func<T, bool>> Criteria =>
        combiner(left.Criteria, right.Criteria);

    public override IQueryable<T> ApplyIncludes(IQueryable<T> query) =>
        right.ApplyIncludes(left.ApplyIncludes(query));

    public override IReadOnlyList<OrderByClause<T>> OrderByClauses =>
        left.OrderByClauses.Count > 0 ? left.OrderByClauses : right.OrderByClauses;

    public override bool UseSplitQuery => left.UseSplitQuery || right.UseSplitQuery;
}

internal sealed class NegatedSpecification<T>(ISpecification<T> inner) : SpecificationBase<T>
{
    public override Expression<Func<T, bool>> Criteria =>
        ExpressionCombiner.Not(inner.Criteria);

    public override IQueryable<T> ApplyIncludes(IQueryable<T> query) =>
        inner.ApplyIncludes(query);

    public override IReadOnlyList<OrderByClause<T>> OrderByClauses => inner.OrderByClauses;

    public override bool UseSplitQuery => inner.UseSplitQuery;
}
