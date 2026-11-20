using Localization.Shared;
using Localization.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.ServiceAbstraction
{
    public interface ILanguageService
    {
        Task<IReadOnlyList<LanguageDto>> GetActiveAsync(CancellationToken ct = default);
        Task<IReadOnlyList<LanguageDto>> GetAllAsync(CancellationToken ct = default);
        Task<Result<LanguageCreatedDto>> CreateAsync(CreateLanguageRequest request, CancellationToken ct = default);
        Task<Result<LanguageDto>> SetActiveAsync(string code, bool isActive, CancellationToken ct = default);
        Task<Result<TranslationJobDto>> RetranslateAsync(string code, CancellationToken ct = default);
    }
}
