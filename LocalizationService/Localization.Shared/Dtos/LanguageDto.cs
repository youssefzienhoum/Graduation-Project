using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared.Dtos
{
    public class LanguageDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string NativeName { get; set; } = null!;
        /// <summary>"ltr" or "rtl"</summary>
        public string Direction { get; set; } = "ltr";
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        /// <summary>None | Pending | InProgress | Completed | Failed</summary>
        public string TranslationStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
