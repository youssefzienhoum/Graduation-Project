using Localization.Domain.Contract;
using Localization.Domain.Entities;
using Localization.Service.Specifications;
using Localization.ServiceAbstraction;
using Localization.ServiceAbstraction.Jobs;
using Localization.Shared;
using Localization.Shared.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service;

internal class LanguageService(IUnitOfWork uow,
ITranslationJobScheduler scheduler,
ILogger<LanguageService> logger) : ILanguageService
{
    private IRepository<Language, Guid> Languages => uow.GetRepository<Language, Guid>();

    public Task<IReadOnlyList<LanguageDto>> GetActiveAsync(CancellationToken ct = default)
        => GetAsync(onlyActive: true, ct);

    public Task<IReadOnlyList<LanguageDto>> GetAllAsync(CancellationToken ct = default)
        => GetAsync(onlyActive: false, ct);

    private async Task<IReadOnlyList<LanguageDto>> GetAsync(bool onlyActive, CancellationToken ct)
    {
        var items = await Languages.GetAllAsync(new LanguagesSpecification(onlyActive), ct);

        return items
            .OrderByDescending(l => l.IsDefault)   
            .ThenBy(l => l.Name)
            .Select(ToDto)
            .ToList();
    }

    public async Task<Result<LanguageCreatedDto>> CreateAsync(CreateLanguageRequest request, CancellationToken ct = default)
    {
        if (!LanguageCodeNormalizer.TryNormalize(request.Code, out var code))
            return Fail<LanguageCreatedDto>(ErrorType.Validation,
                "Invalid language code. Use a BCP-47 code such as 'fr', 'pt-BR' or 'zh-Hans'.");

        TextDirection? requestedDirection = null;
        if (!string.IsNullOrWhiteSpace(request.Direction))
        {
            requestedDirection = request.Direction.Trim().ToLowerInvariant() switch
            {
                "ltr" => TextDirection.LeftToRight,
                "rtl" => TextDirection.RightToLeft,
                _ => null
            };
            if (requestedDirection is null)
                return Fail<LanguageCreatedDto>(ErrorType.Validation, "Direction must be 'ltr' or 'rtl'.");
        }

        if (await Languages.GetByIdAsync(new LanguageByCodeSpecification(code), ct) is not null)
            return Fail<LanguageCreatedDto>(ErrorType.Conflict, $"Language '{code}' already exists.");

        //if (await Languages.GetByIdAsync(new DefaultLanguageSpecification(), ct) is null)
        //    return Fail<LanguageCreatedDto>(ErrorType.Conflict,
        //        "No default language is configured, so there is no source to translate from.");

        var culture = TryGetCulture(code);
        var name = request.Name?.Trim();
        var nativeName = request.NativeName?.Trim();
        if (string.IsNullOrEmpty(name)) name = culture?.EnglishName;
        if (string.IsNullOrEmpty(nativeName) && culture is not null)
            nativeName = culture.TextInfo.ToTitleCase(culture.NativeName);

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(nativeName))
            return Fail<LanguageCreatedDto>(ErrorType.Validation,
                $"Could not detect the names for '{code}'. Please provide 'name' and 'nativeName'.");

        var language = new Language
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            NativeName = nativeName,
            Direction = requestedDirection
                        ?? (culture?.TextInfo.IsRightToLeft == true ? TextDirection.RightToLeft : TextDirection.LeftToRight),
            IsEnabled = false,                               // stays inactive until translation is complete
            IsDefault = false,
            TranslationStatus = TranslationJobStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        Languages.Add(language);
        await uow.SaveChangesAsync(ct);                      // save FIRST so the job can find the row

        string? jobId = null;
        try
        {
            jobId = scheduler.EnqueueLanguageTranslation(code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Language '{Language}' was created but its translation job could not be enqueued.", code);
            language.TranslationStatus = TranslationJobStatus.Failed;
            await uow.SaveChangesAsync(ct);                  
        }

        return Result<LanguageCreatedDto>.Success(new LanguageCreatedDto { Language = ToDto(language), JobId = jobId });
    }


    public async Task<Result<LanguageDto>> SetActiveAsync(string code, bool isActive, CancellationToken ct = default)
    {
        var (language, error) = await FindAsync<LanguageDto>(code, ct);
        if (language is null) return error!;

        if (language.IsEnabled == isActive)
            return Result<LanguageDto>.Success(ToDto(language));        

        if (!isActive && language.IsDefault)
            return Fail<LanguageDto>(ErrorType.Conflict, "The default language cannot be deactivated.");

        if (isActive && !language.IsDefault && language.TranslationStatus != TranslationJobStatus.Completed)
            return Fail<LanguageDto>(ErrorType.Conflict,
                $"Language '{language.Code}' cannot be activated until its translation is completed " +
                $"(current status: {language.TranslationStatus}).");

        language.IsEnabled = isActive;
        await uow.SaveChangesAsync(ct);

        return Result<LanguageDto>.Success(ToDto(language));
    }


    public async Task<Result<TranslationJobDto>> RetranslateAsync(string code, CancellationToken ct = default)
    {
        var (language, error) = await FindAsync<TranslationJobDto>(code, ct);
        if (language is null) return error!;

        if (language.IsDefault)
            return Fail<TranslationJobDto>(ErrorType.Conflict,
                "The default language is the translation source and cannot be retranslated.");

        if (language.TranslationStatus is TranslationJobStatus.Pending or TranslationJobStatus.InProgress)
            return Fail<TranslationJobDto>(ErrorType.Conflict,
                $"A translation job for '{language.Code}' is already {language.TranslationStatus}.");

        language.TranslationStatus = TranslationJobStatus.Pending;
        await uow.SaveChangesAsync(ct);

        try
        {
            var jobId = scheduler.EnqueueLanguageTranslation(language.Code);
            return Result<TranslationJobDto>.Success(new TranslationJobDto
            {
                JobId = jobId,
                LanguageCode = language.Code,
                Status = TranslationJobStatus.Pending.ToString()
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not enqueue the retranslation job for '{Language}'.", language.Code);
            language.TranslationStatus = TranslationJobStatus.Failed;
            await uow.SaveChangesAsync(ct);
            return Fail<TranslationJobDto>(ErrorType.Unavailable, "The translation job could not be enqueued. Try again later.");
        }
    }


    private async Task<(Language? Language, Result<T>? Error)> FindAsync<T>(string code, CancellationToken ct)
    {
        if (!LanguageCodeNormalizer.TryNormalize(code, out var normalized))
            return (null, Fail<T>(ErrorType.Validation, "Invalid language code."));

        var language = await Languages.GetByIdAsync(new LanguageByCodeSpecification(normalized), ct);
        return language is null
            ? (null, Fail<T>(ErrorType.NotFound, $"Language '{normalized}' was not found."))
            : (language, null);
    }

    private static Result<T> Fail<T>(ErrorType type, string message) => Result<T>.Failure(type, message);

    private static CultureInfo? TryGetCulture(string code)
    {
        try
        {
            var culture = CultureInfo.GetCultureInfo(code);
            // ICU can return a placeholder culture for unknown codes; ignore those.
            return string.IsNullOrEmpty(culture.Name)
                   || string.Equals(culture.EnglishName, code, StringComparison.OrdinalIgnoreCase)
                ? null
                : culture;
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
    }

    private static LanguageDto ToDto(Language l) => new()
    {
        Id = l.Id,
        Code = l.Code,
        Name = l.Name,
        NativeName = l.NativeName,
        Direction = l.Direction == TextDirection.RightToLeft ? "rtl" : "ltr",
        IsActive = l.IsEnabled,
        IsDefault = l.IsDefault,
        TranslationStatus = l.TranslationStatus.ToString(),
        CreatedAt = l.CreatedAt
    };
}