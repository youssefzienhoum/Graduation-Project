using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.ServiceAbstraction.Jobs
{
    public interface ILanguageTranslationJob
    {
        Task RunAsync(
           string languageCode,
           CancellationToken ct
       );
    }
}
