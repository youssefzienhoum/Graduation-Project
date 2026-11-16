using Chat.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Context.Configuration
{
    public class ConversationParticipantConfguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasIndex(x => new
            {
                x.ConversationId,
                x.UserId
            }).IsUnique();
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(100);    
            builder.Property(x => x.ProfileImageUrl).HasMaxLength(500);
            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            
            builder.Property(x => x.ConversationId).IsRequired();
        }
    }
}
