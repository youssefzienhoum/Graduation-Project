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
    internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(35)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NativeName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Direction).IsRequired();

            builder.Property(x => x.IsEnabled)
                .HasDefaultValue(true);

            builder.Property(x => x.IsDefault)
                .HasDefaultValue(false);
            builder.Property(x => x.TranslationStatus).IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.HasIndex(x => x.IsDefault);

            

            // Language -> TranslationValues (1:M)

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Language)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            // Language -> TranslationBundles (1:M)
            builder.HasMany(x => x.Bundles)
                .WithOne(x => x.Language)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
