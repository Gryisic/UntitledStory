using System;
using System.Collections.Generic;
using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Stats
{
    public class StatsHandler : IStatsHandler
    {
        private readonly Dictionary<Enums.UnitStat, Stat> _statsMap;

        public StatsHandler(StatDataContainer dataContainer)
        {
            _statsMap = new Dictionary<Enums.UnitStat, Stat>();
            
            Initialize(dataContainer);
        }

        private void Initialize(StatDataContainer dataContainer)
        {
            foreach (var unitStat in dataContainer.DataMap.Keys)
            {
                Stat stat = new Stat(dataContainer.DataMap[unitStat]);
                
                _statsMap.Add(unitStat, stat);
            }
        }

        public void AddModifierToStat(Enums.UnitStat stat, IStatModifier modifier) 
            => _statsMap[stat].AddModifier(modifier);

        public void ClearStat(Enums.UnitStat stat) => _statsMap[stat].Clear();

        public bool TryRemoveModifierFromStat(Enums.UnitStat stat, IStatModifier modifier) 
            => _statsMap[stat].TryRemoveModifier(modifier);
        
        public bool TryRemoveAllModifierFromSourceInStat(Enums.UnitStat stat, IStatModifierSource source) 
            => _statsMap[stat].TryRemoveAllModifiersFromSource(source);

        public void IncreaseStat(Enums.UnitStat stat, int amount) => _statsMap[stat].IncreaseValue(amount);

        public void DecreaseStat(Enums.UnitStat stat, int amount) => _statsMap[stat].DecreaseValue(amount);

        public void IncreaseBaseStat(Enums.UnitStat stat, int amount) => _statsMap[stat].IncreaseBaseValue(amount);
        
        public void DecreaseBaseStat(Enums.UnitStat stat, int amount) => _statsMap[stat].DecreaseBaseValue(amount);

        public IStatData GetStatData(Enums.UnitStat stat)
        {
            if (_statsMap.TryGetValue(stat, out Stat data))
                return data;

            throw new NullReferenceException($"Data map doesn't contain stat of type '{stat}'");
        }
    }
}