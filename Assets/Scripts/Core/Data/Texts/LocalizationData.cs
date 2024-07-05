using Core.Data.Interfaces;
using Newtonsoft.Json;

namespace Core.Data.Texts
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class LocalizationData : ILocalizableTextData
    {
        [JsonProperty(PropertyName = "ID")] private int _id;
        [JsonProperty(PropertyName = "Key")] private string _key;
        [JsonProperty(PropertyName = "Name")] private string _name;

        public int ID => _id;
        public string Key => _key;
        public string Name => _name;
    }
}