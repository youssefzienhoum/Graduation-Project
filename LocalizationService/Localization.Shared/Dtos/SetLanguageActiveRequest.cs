using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Shared.Dtos
{
    public class SetLanguageActiveRequest
    {
        // nullable + [Required] so a missing field is rejected instead of silently meaning "false"
        [Required]
        public bool? IsActive { get; set; }
    }
}
