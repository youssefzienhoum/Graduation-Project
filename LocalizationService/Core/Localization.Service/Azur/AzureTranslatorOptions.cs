using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localization.Service.Azur
{
    public class AzureTranslatorOptions
    {
        public const string SectionName = "AzureTranslator";

        public string Endpoint { get; set; } = "https://api.cognitive.microsofttranslator.com";
        public string Key { get; set; } = "";
        /// <summary>Required for regional / multi-service Translator resources.</summary>
        public string? Region { get; set; }
    }
}
