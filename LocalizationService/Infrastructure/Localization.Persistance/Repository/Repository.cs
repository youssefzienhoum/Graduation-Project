using Localization.Domain.Contract;
using Localization.Domain.Entities;
using Localization.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Persistence.Repository
{
    public class Repository<TEntity, TKey>(TranslationDbContext translationDbContext) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)
            => translationDbContext.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await translationDbContext.Set<TEntity>().GetQuery(specification).CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await translationDbContext.Set<TEntity>().GetQuery(specification).ToListAsync(cancellationToken);
        }



        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await translationDbContext.Set<TEntity>().GetQuery(specification).FirstOrDefaultAsync(cancellationToken);

        }


        public void Remove(TEntity entity)
            => translationDbContext.Set<TEntity>().Remove(entity);


        public void Update(TEntity entity)
            => translationDbContext.Set<TEntity>().Update(entity);

        public async Task<Dictionary<Guid, int>> CountByAsync(ISpecification<TEntity> spec,
             Expression<Func<TEntity, Guid>> groupBy
            , CancellationToken ct = default)
        {
            return await translationDbContext
                            .Set<TEntity>()
                            .GetQuery(spec)
                            .GroupBy(groupBy)
                            .Select(g => new { IssueId = g.Key, Count = g.Count() })
                            .ToDictionaryAsync(x => x.IssueId, x => x.Count, ct);
        }

    }
}

