using Localization.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Specifications;

public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
    {
        Criteria = expression;
    }
    public Expression<Func<TEntity, bool>> Criteria { get; protected set; } = null!;
    public ICollection<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
    public ICollection<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeChains { get; } = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();
    public Expression<Func<TEntity, object>> OrderBy { get; protected set; } = null!;
    public Expression<Func<TEntity, object>> OrderByDesc { get; protected set; } = null!;
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPaginated { get; private set; }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginated = true;
    }
    protected void AddOrderBy(Expression<Func<TEntity, object>> ordertby) =>
            OrderBy = ordertby;


    protected void AddOrderByDesc(Expression<Func<TEntity, object>> expression) =>
        OrderByDesc = expression;
}

