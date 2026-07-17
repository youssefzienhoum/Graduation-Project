using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities
{
    public  class AiAnalysis: BaseEntity<Guid>
    {
        public double Confidence { get; set; }
        public string DiagnosisText { get; set; } = string.Empty;
        public SeverityLevel Severity { get; set; }
        public string? RecommendedAction { get; set; }
        public string? ModelVersion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Problem Problem { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; } = null!;
    }
}
