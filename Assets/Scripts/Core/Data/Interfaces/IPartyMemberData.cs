using Common.Models.Skills.Interfaces;
using Common.Models.Stats.Interfaces;

namespace Core.Data.Interfaces
{
    public interface IPartyMemberData
    {
        IStatsHandler StatsHandler { get; }
        ISkillsHandler SkillsHandler { get; }
    }
}