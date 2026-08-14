using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Context.Configuration
{
    public class IssueVoteConfiguration : IEntityTypeConfiguration<IssueVote>
    {
        public void Configure(EntityTypeBuilder<IssueVote> builder)
        {
            builder
                .HasKey(iv => iv.Id);
            builder
                .Property(x=>x.IssueId)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();
            builder
                .HasIndex(x => new { x.IssueId, x.UserId })
                .IsUnique();
        }
    }
}
