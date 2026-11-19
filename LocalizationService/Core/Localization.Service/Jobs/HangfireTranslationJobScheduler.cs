using Hangfire;
using Localization.ServiceAbstraction.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Jobs
{
    public class HangfireTranslationJobScheduler : ITranslationJobScheduler
    {
        private readonly IBackgroundJobClient _jobClient;

        public HangfireTranslationJobScheduler(
            IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        public string EnqueueLanguageTranslation(
            string languageCode)
        {
            return _jobClient.Enqueue<ILanguageTranslationJob>(
                job => job.RunAsync(
                    languageCode,
                    CancellationToken.None
                )
            );
        }
    }
}
