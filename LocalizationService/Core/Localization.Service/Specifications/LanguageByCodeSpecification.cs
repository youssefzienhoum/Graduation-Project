using Localization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Specifications;


    public sealed class LanguageByCodeSpecification : BaseSpecification<Language>
    {
        public LanguageByCodeSpecification(string code)
        {
            Criteria = l => l.Code == code;
        }
    }

    public sealed class DefaultLanguageSpecification : BaseSpecification<Language>
    {
        public DefaultLanguageSpecification()
        {
            Criteria = l => l.IsDefault;
        }
    }
    public sealed class SourceValuesMissingInTargetSpecification : BaseSpecification<TranslationValue>
    {
        public SourceValuesMissingInTargetSpecification(Guid sourceLanguageId, Guid targetLanguageId, int batchSize)
        {
            Criteria = v =>
                v.LanguageId == sourceLanguageId &&
                v.Value != "" &&
                !v.Key.Translations.Any(t => t.LanguageId == targetLanguageId);

            OrderBy = v => v.KeyId;          // deterministic batches
            ApplyPaging(0, batchSize);
        }
    }


