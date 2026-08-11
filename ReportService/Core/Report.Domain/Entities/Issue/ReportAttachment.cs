using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Issue

{
    public class ReportAttachment : BaseEntity<Guid>
    {
        public ReportAttachmentType Type { get; set; }
        public string Url { get; set; } = null!;

        public Guid IssueId { get; set; }
        public Issue Issue { get; set; } = null!;
 
    }
}
