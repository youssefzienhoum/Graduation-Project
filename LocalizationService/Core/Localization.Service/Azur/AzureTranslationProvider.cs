using Localization.ServiceAbstraction.Azur;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Localization.Service.Azur
{
    public class AzureTranslationProvider(
        HttpClient http,
        IOptions<AzureTranslatorOptions> options,
        ILogger<AzureTranslationProvider> logger) : ITranslationProvider
    {
        private const int MaxAttempts = 3;
        private readonly AzureTranslatorOptions _options = options.Value;

        public async Task<IReadOnlyList<string>> TranslateAsync(
            IReadOnlyList<string> texts, string sourceLanguage, string targetLanguage, CancellationToken ct = default)
        {
            if (texts.Count == 0) return [];

            if (string.IsNullOrWhiteSpace(_options.Key))
                throw new InvalidOperationException("AzureTranslator:Key is not configured.");

            // Translator treats the text as HTML so we can protect {placeholders} with translate="no".
            var url = $"translate?api-version=3.0&from={Uri.EscapeDataString(sourceLanguage)}" +
                      $"&to={Uri.EscapeDataString(targetLanguage)}&textType=html";
            var body = texts.Select(t => new { Text = Protect(t) }).ToArray();

            for (var attempt = 1; ; attempt++)
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = JsonContent.Create(body) };
                request.Headers.Add("Ocp-Apim-Subscription-Key", _options.Key);
                if (!string.IsNullOrWhiteSpace(_options.Region))
                    request.Headers.Add("Ocp-Apim-Subscription-Region", _options.Region);

                using var response = await http.SendAsync(request, ct);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<TranslateResult>>(cancellationToken: ct)
                                 ?? throw new InvalidOperationException("Empty response from Azure Translator.");

                    return result.Select(r => Restore(r.Translations.First().Text)).ToList();
                }

                var transient = response.StatusCode == HttpStatusCode.TooManyRequests || (int)response.StatusCode >= 500;
                if (transient && attempt < MaxAttempts)
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    logger.LogWarning("Azure Translator returned {Status}, retrying in {Delay}s (attempt {Attempt}).",
                        (int)response.StatusCode, delay.TotalSeconds, attempt);
                    await Task.Delay(delay, ct);
                    continue;
                }

                var error = await response.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"Azure Translator failed ({(int)response.StatusCode}): {error}");
            }
        }

        // "Hello {name} & co" -> "Hello <span translate="no">{name}</span> &amp; co"
        private static readonly Regex PlaceholderRegex = new(@"\{[^{}]+\}", RegexOptions.Compiled);
        private static readonly Regex SpanRegex = new(@"</?span[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // "Hello {name} & co" -> "Hello <span translate="no">{name}</span> &amp; co"
        private static string Protect(string text) =>
            PlaceholderRegex.Replace(WebUtility.HtmlEncode(text), "<span translate=\"no\">$0</span>");

        private static string Restore(string translated) =>
            WebUtility.HtmlDecode(SpanRegex.Replace(translated, ""));

        private sealed record TranslateResult([property: JsonPropertyName("translations")] List<TranslatedText> Translations);
        private sealed record TranslatedText([property: JsonPropertyName("text")] string Text);
    }
}
