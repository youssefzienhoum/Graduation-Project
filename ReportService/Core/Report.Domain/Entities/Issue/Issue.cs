using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Report.Domain.Entities.Issue

{
    public  class Issue:BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty; // e.g. "leak", "distribution_problem"
        public IssueStatus Status { get; set; } = IssueStatus.Reported;
        public IssuePriority Priority { get; set; } = IssuePriority.Medium;
        public Guid ReporterId { get; set; } // user who reported the issue
        public Guid? AssignedExpertId { get; set; } // expert assigned to the issue
     
    
        public Guid? LocationId { get; set; }
        public GPSLocation GPSLocation { get; set; }
        public ICollection<AiAnalysis> aiAnalyses { get; set; } =new List<AiAnalysis>();
        public ICollection<ReportAttachment> reportAttachments { get; set; } = new List<ReportAttachment>();


    }
}
