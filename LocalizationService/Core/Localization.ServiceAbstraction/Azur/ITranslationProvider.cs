using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.ServiceAbstraction.Azur
{
    public interface ITranslationProvider
    {
        Task<IReadOnlyList<string>> TranslateAsync(
            IReadOnlyList<string> texts,
            string sourceLanguage,
            string targetLanguage,
            CancellationToken ct = default);
    }
}
