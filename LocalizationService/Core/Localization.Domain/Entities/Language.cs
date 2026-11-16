using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Domain.Entities
{
    public class Language: BaseEntity<Guid>
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string NativeName { get; set; } = null!;

        public TextDirection Direction { get; set; } = TextDirection.LeftToRight;

        public bool IsEnabled { get; set; } = true;

        public bool IsDefault { get; set; }


        public ICollection<TranslationValue> Translations { get; set; }
            = new List<TranslationValue>();

        public ICollection<TranslationBundle> Bundles { get; set; }
            = new List<TranslationBundle>();
    }
}
