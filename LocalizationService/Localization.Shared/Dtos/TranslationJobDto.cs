using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared.Dtos
{
    public class TranslationJobDto
    {
        public string JobId { get; set; } = null!;
        public string LanguageCode { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
