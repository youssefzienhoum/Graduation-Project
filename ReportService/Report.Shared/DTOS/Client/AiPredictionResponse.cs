using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Shared.DTOS.Client
{
    public  record AiPredictionResponse(
        string Problem,
        string ProblemArabic,
        double Confidence,
        string Severity,
        string Recommendation,
        string Explanation,
        List<string> RepairSteps,
        string ModelVersion,
        string Urgency
        )
    {
    }
}
