using Localization.Domain.Contract;
using Localization.Domain.Entities;
using Localization.Service.Specifications;
using Localization.ServiceAbstraction.Azur;
using Localization.ServiceAbstraction.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Jobs;

public class LanguageTranslationJob(
    IUnitOfWork uow,
    ITranslationProvider provider,
    IServiceScopeFactory scopeFactory,
    ILogger<LanguageTranslationJob> logger)
    : ILanguageTranslationJob
{
    private const int BatchSize = 50;

    public async Task RunAsync(string languageCode, CancellationToken ct)
    {
        var languages = uow.GetRepository<Language, Guid>();

        var target = await languages.GetByIdAsync(new LanguageByCodeSpecification(languageCode), ct);
        if (target is null)
        {
            logger.LogWarning("Translation job: language '{Language}' not found.", languageCode);
            return;
        }

        if (target.IsDefault)
        {
            logger.LogWarning("Translation job: '{Language}' is the default language, nothing to translate.", languageCode);
            return;
        }

        if (target.TranslationStatus == TranslationJobStatus.InProgress)
        {
            logger.LogWarning("Translation job for '{Language}' is already running. Skipping.", languageCode);
            return;
        }

        try
        {
            var source = await languages.GetByIdAsync(new DefaultLanguageSpecification(), ct)
                ?? throw new InvalidOperationException("No default language is configured.");

            target.TranslationStatus = TranslationJobStatus.InProgress;
            await uow.SaveChangesAsync(ct);

            await TranslateMissingAsync(source, target, ct);

            target.TranslationStatus = TranslationJobStatus.Completed;
            await uow.SaveChangesAsync(ct);

            logger.LogInformation("Translation to '{Language}' completed.", languageCode);
        }
        catch (OperationCanceledException)
        {
            // App is shutting down: Hangfire will re-run the job, so go back to Pending (not stuck InProgress).
            await SetStatusInNewScopeAsync(languageCode, TranslationJobStatus.Pending);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Auto-translation to '{Language}' failed.", languageCode);
            // New scope on purpose: the failed DbContext may still track half-saved TranslationValues.
            await SetStatusInNewScopeAsync(languageCode, TranslationJobStatus.Failed);
        }
    }

    private async Task TranslateMissingAsync(Language source, Language target, CancellationToken ct)
    {
        var values = uow.GetRepository<TranslationValue, Guid>();

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            var batch = (await values.GetAllAsync(
                new SourceValuesMissingInTargetSpecification(source.Id, target.Id, BatchSize), ct)).ToList();

            if (batch.Count == 0)
                return;

            var translated = await provider.TranslateAsync(
                batch.Select(v => v.Value).ToList(), source.Code, target.Code, ct);

            if (translated.Count != batch.Count)
                throw new InvalidOperationException("Translation provider returned an unexpected number of results.");

            var now = DateTime.UtcNow;
            for (var i = 0; i < batch.Count; i++)
            {
                values.Add(new TranslationValue
                {
                    KeyId = batch[i].KeyId,
                    LanguageId = target.Id,
                    Value = translated[i],
                    IsMachineTranslated = true,
                    IsReviewed = false,
                    UpdatedAt = now
                });
            }

            await uow.SaveChangesAsync(ct);   // saved per batch => progress survives a crash
        }
    }

    private async Task SetStatusInNewScopeAsync(string languageCode, TranslationJobStatus status)
    {
        using var scope = scopeFactory.CreateScope();
        var scopedUow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var language = await scopedUow.GetRepository<Language, Guid>()
            .GetByIdAsync(new LanguageByCodeSpecification(languageCode));
        if (language is null) return;

        language.TranslationStatus = status;
        await scopedUow.SaveChangesAsync();   // CancellationToken.None on purpose
    }
}