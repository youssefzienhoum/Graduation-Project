using Localization.Domain.Contract;
using Localization.Domain.Entities;
using Localization.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Persistence.Repository
{
    public class UnitOfWork(TranslationDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = [];

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (repositories.ContainsKey(type))
                return (IRepository<TEntity, TKey>)repositories[type];

            var repo = new Repository<TEntity, TKey>(dbContext);
            repositories.Add(type, repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
       => await dbContext.SaveChangesAsync(cancellationToken);


    }
}
