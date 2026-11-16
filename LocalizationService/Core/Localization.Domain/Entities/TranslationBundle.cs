using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Domain.Entities
{
    public class TranslationBundle:BaseEntity<Guid>
    {
        public Guid LanguageId { get; set; }

        public string Namespace { get; set; } = null!;

        public int Version { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string ContentJson { get; set; } = null!;

        public Language Language { get; set; } = null!;
    }
}
