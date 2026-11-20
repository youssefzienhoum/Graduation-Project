using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Localization.Service;

public static class LanguageCodeNormalizer
{
    private static readonly Regex Pattern =
    new(@"^[A-Za-z]{2,3}(-[A-Za-z]{4})?(-([A-Za-z]{2}|[0-9]{3}))?$", RegexOptions.Compiled);

    public static bool TryNormalize(string? code, out string normalized)
    {
        normalized = string.Empty;
        if (string.IsNullOrWhiteSpace(code)) return false;

        var trimmed = code.Trim();
        if (!Pattern.IsMatch(trimmed)) return false;

        var parts = trimmed.Split('-');
        for (var i = 0; i < parts.Length; i++)
        {
            parts[i] = i == 0
                ? parts[i].ToLowerInvariant()
                : parts[i].Length == 4
                    ? char.ToUpperInvariant(parts[i][0]) + parts[i][1..].ToLowerInvariant()
                    : parts[i].ToUpperInvariant();
        }

        normalized = string.Join('-', parts);
        return true;
    }
}