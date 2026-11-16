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
    internal class TranslationValueConfiguration
        : IEntityTypeConfiguration<TranslationValue>
    {
        public void Configure(EntityTypeBuilder<TranslationValue> builder)
        {
           

            builder.HasKey(x => x.Id);

           ;

            builder.Property(x => x.Value)
                .HasColumnType("longtext")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.KeyId,
                x.LanguageId
            }).IsUnique();

            builder.HasOne(x => x.Key)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.KeyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Language)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}
