using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Data.Texts
{
    public class PartyMembersLocalization : LocalizationProvider<PartyMemberLocalization> { }
    
    public class PartyMemberLocalization : LocalizationData
    {
        [JsonProperty(PropertyName = "ActionPhrase")] private string _actionPhrase;
        [JsonProperty(PropertyName = "BattleThoughts")] private BattleThoughtsLocalization _thoughts;

        public string ActionPhrase => _actionPhrase;
        public BattleThoughtsLocalization Thoughts => _thoughts;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BattleThoughtsLocalization
    {
        [JsonProperty(PropertyName = "Start")] private List<string> _start;
        [JsonProperty(PropertyName = "Win")] private List<string> _win;
        [JsonProperty(PropertyName = "Neutral")] private List<string> _neutral;

        public IReadOnlyList<string> Start => _start;
        public IReadOnlyList<string> Win => _win;
        public IReadOnlyList<string> Neutral => _neutral;
    }
}