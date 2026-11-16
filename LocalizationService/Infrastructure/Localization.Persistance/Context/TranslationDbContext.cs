using Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Persistence.Context
{
    public class TranslationDbContext( DbContextOptions<TranslationDbContext> options)
    : DbContext(options)
    {

        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<TranslationKey> TranslationKeys { get; set; } = null!;
        public DbSet<TranslationValue> TranslationValues { get; set; } = null!;
        public DbSet<TranslationBundle> TranslationBundles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TranslationDbContext).Assembly);
        }


    }
}
