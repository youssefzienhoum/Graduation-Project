using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.ServiceAbstraction.Azur
{
    public class FakeProvider : Localization.ServiceAbstraction.Azur.ITranslationProvider
    {
        public Task<IReadOnlyList<string>> TranslateAsync(IReadOnlyList<string> texts, string s, string t, CancellationToken ct = default)
            => Task.FromResult<IReadOnlyList<string>>(texts.Select(x => $"[{t}] {x}").ToList());
    }
}
