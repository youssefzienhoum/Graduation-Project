using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Report
{
    public  record CreateIssueResponse(Guid Id,
    string? Description,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid ReporterId
        )
    {
    }
}
