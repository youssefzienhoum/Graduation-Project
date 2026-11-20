using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared.Dtos
{
    public class LanguageCreatedDto
    {
        public LanguageDto Language { get; set; } = null!;
        /// <summary>Hangfire job id, or null if the job could not be enqueued (status will be Failed).</summary>
        public string? JobId { get; set; }
    }
}
