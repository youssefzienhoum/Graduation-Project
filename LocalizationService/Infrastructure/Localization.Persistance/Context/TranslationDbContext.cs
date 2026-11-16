using Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Persistence.Context
{
    internal class TranslationDbContext( DbContextOptions<TranslationDbContext> options)
    : DbContext(options)
    {

        public DbSet<Language> Languages => Set<Language>();
        public DbSet<TranslationKey> TranslationKeys => Set<TranslationKey>();
        public DbSet<TranslationValue> TranslationValues => Set<TranslationValue>();
        public DbSet<TranslationBundle> TranslationBundles => Set<TranslationBundle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TranslationDbContext).Assembly);
        }


    }
}
