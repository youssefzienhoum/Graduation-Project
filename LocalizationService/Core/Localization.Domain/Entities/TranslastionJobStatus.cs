using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Domain.Entities
{
    public enum TranslationJobStatus
    {
        None = 0,
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Failed = 4
    }
}
