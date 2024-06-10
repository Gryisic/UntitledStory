using Infrastructure.Utils;
using Infrastructure.Utils.Types;

namespace Common.Models.Stats.Interfaces
{
    public interface IStatsHandler
    {
        void AddModifierToStat(Enums.UnitStat stat, IStatModifier modifier);
        void ClearStat(Enums.UnitStat stat);
        bool TryRemoveModifierFromStat(Enums.UnitStat stat, IStatModifier modifier);
        bool TryRemoveAllModifierFromSourceInStat(Enums.UnitStat stat, IStatModifierSource source);

        void IncreaseStat(Enums.UnitStat stat, int amount);
        void DecreaseStat(Enums.UnitStat stat, int amount);
        void IncreaseBaseStat(Enums.UnitStat stat, int amount);
        void DecreaseBaseStat(Enums.UnitStat stat, int amount);
        
        IStatData GetStatData(Enums.UnitStat stat);
    }
}