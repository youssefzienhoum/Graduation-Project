using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Entities.ISSUE
{
    // Must match Report.Domain.Entities.Issue.IssuePriority: both read the same
    // Issues.Priority column, and ReportService is the only writer.
    public enum IssuePriority
    {
        Unknown = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4,

    }
}
