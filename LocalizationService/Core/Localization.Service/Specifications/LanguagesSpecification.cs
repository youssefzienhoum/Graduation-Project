using Localization.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Specifications
{
    internal class LanguagesSpecification :BaseSpecification<Language>
    {
        public LanguagesSpecification(bool IsEnabled) :base(l => l.IsEnabled == IsEnabled)
        {
            AddOrderBy(l => l.Name);
        }
    }
}
