using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared.Dtos
{
    public class CreateLanguageRequest
    {
        /// <summary>BCP-47 code, e.g. "fr", "pt-BR", "zh-Hans".</summary>
        [Required, StringLength(35, MinimumLength = 2)]
    public string Code { get; set; } = null!;

    /// <summary>English name. Optional: taken from the code when omitted.</summary>
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>Native name. Optional: taken from the code when omitted.</summary>
    [StringLength(100)]
    public string? NativeName { get; set; }

    /// <summary>"ltr" or "rtl". Optional: detected from the code when omitted.</summary>
    [RegularExpression("^(?i)(ltr|rtl)$", ErrorMessage = "Direction must be 'ltr' or 'rtl'.")]
        public string? Direction { get; set; }
    }
}
