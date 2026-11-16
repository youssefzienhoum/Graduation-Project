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
    internal class TranslationKeyConfiguration : IEntityTypeConfiguration<TranslationKey>
    {
        public void Configure(EntityTypeBuilder<TranslationKey> builder)
        {
            {
                builder.HasKey(x => x.Id);
;

                builder.Property(x => x.Key)
                    .HasMaxLength(200)
                    .IsRequired();

                builder.Property(x => x.Namespace)
                    .HasMaxLength(100)
                    .IsRequired();

                builder.Property(x => x.Description)
                    .HasMaxLength(500);

                builder.Property(x => x.CreatedAt)
                    .IsRequired();

                builder.HasIndex(x => new
                {
                    x.Namespace,
                    x.Key
                }).IsUnique();

                // TranslationKey -> TranslationValues (1:M)
                builder.HasMany(x => x.Translations)
                        .WithOne(x => x.Key)
                        .HasForeignKey(x => x.KeyId)
                        .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
