using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Entities.ReadModels
{ 
    public class ExpertInboxReadModel
    {
        [Key]
        public Guid IssueId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        [MaxLength(250)]
        public string? Description { get; set; }

        public IssueStatus Status { get; set; }

        public IssuePriority Priority { get; set; }
        [MaxLength(200)]
        public string? ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? AssignedExpertId { get; set; }
        [MaxLength(100)]
        public string? AssignedExpertName { get; set; }
    }
}
