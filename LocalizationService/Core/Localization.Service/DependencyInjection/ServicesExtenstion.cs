using Hangfire;
using Localization.Service.Azur;
using Localization.Service.Jobs;
using Localization.ServiceAbstraction;
using Localization.ServiceAbstraction.Azur;
using Localization.ServiceAbstraction.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.DependencyInjection
{
    public static class ServicesExtenstion
    {
        public static IServiceCollection AddLocalizationService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
           configuration.GetConnectionString("Hangfire");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    "Hangfire connection string is missing.");

            services.AddHangfire(config =>
            {
                config
                    .SetDataCompatibilityLevel(
                        CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString);
            });

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 2;
            });

            services.Configure<AzureTranslatorOptions>(configuration.GetSection(AzureTranslatorOptions.SectionName));
            services.AddHttpClient<ITranslationProvider, AzureTranslationProvider>((sp, client) =>
            {
                var endpoint = configuration[$"{AzureTranslatorOptions.SectionName}:Endpoint"]
                               ?? new AzureTranslatorOptions().Endpoint;
                client.BaseAddress = new Uri(endpoint.TrimEnd('/') + "/");
                client.Timeout = TimeSpan.FromSeconds(60);
            });
            services.AddScoped<
                ITranslationJobScheduler,
                HangfireTranslationJobScheduler>();
            services.AddScoped<ILanguageTranslationJob, LanguageTranslationJob>();
            services.AddScoped<ITranslationProvider, AzureTranslationProvider>();
            services.AddScoped<ILanguageService, LanguageService>();
            return services;
        }
    }
}
