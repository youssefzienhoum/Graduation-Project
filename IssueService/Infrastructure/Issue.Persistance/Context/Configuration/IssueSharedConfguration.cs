using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Context.Configuration
{
    public class IssueSharedConfguration : IEntityTypeConfiguration<IssueShared>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IssueShared> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IssueId)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();
            builder.HasOne(i => i.Issue)
                .WithMany(s => s.Shares)
                .OnDelete(DeleteBehavior.Cascade); ;

        }
    }
}
