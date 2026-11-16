using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Domain.Entities
{
    public class TranslationKey:BaseEntity<Guid>
    {

        public string Key { get; set; } = null!;

        public string Namespace { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TranslationValue> Translations { get; set; }
            = new List<TranslationValue>();
    }
}
