using Localization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Specifications;


    public sealed class LanguageByCodeSpecification : BaseSpecification<Language>
    {
        public LanguageByCodeSpecification(string code) : base(c=> c.Code == code)
        {
            
        }
    }

    public sealed class DefaultLanguageSpecification : BaseSpecification<Language>
    {
        public DefaultLanguageSpecification(bool IsDefault) : base(c => (c.IsDefault == IsDefault))
        {
            
        }
    }
    public sealed class SourceValuesMissingInTargetSpecification : BaseSpecification<TranslationValue>
    {
        public SourceValuesMissingInTargetSpecification(Guid sourceLanguageId, Guid targetLanguageId, int batchSize) :base(v=> v.LanguageId == sourceLanguageId &&
                v.Value != "" &&
                !v.Key.Translations.Any(t => t.LanguageId == targetLanguageId))
    {

        AddOrderBy(v => v.KeyId);
            ApplyPaging(0, batchSize);
        }
    }


