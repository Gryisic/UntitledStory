using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Data.Texts
{
    public class MenusLocalization : LocalizationProvider<GeneralMenuLocalization> { }

    public class GeneralMenuLocalization : LocalizationData
    {
        [JsonProperty(PropertyName = "Entries")] private Dictionary<string, string> _entries;

        public Dictionary<string, string> Entries => _entries;
    }
}