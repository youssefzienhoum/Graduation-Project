


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Domain.Entities
{
    public class TranslationValue:BaseEntity<Guid>
    {
        public Guid KeyId { get; set; }

        public Guid LanguageId { get; set; }

        public string Value { get; set; } = null!;

        public DateTime UpdatedAt { get; set; } 

        public TranslationKey Key { get; set; } = null!;

        public bool IsMachineTranslated { get; set; }
        public bool IsReviewed { get; set; }
        public Language Language { get; set; } = null!;
    }
}
