using Localization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Persistence.Context.Configuration
{
    internal class TranslationBundleConfiguration : IEntityTypeConfiguration<TranslationBundle>
    {
        public void Configure(EntityTypeBuilder<TranslationBundle> builder)
        {
   

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Namespace)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Version)
                .IsRequired();

            builder.Property(x => x.IsPublished)
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.LanguageId,
                x.Namespace,
                x.Version
            }).IsUnique();

            builder.HasOne(x => x.Language)
                .WithMany(x => x.Bundles)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
