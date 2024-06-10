using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Types;
using UnityEngine;

namespace Common.Models.Stats
{
    public class StatDataContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<Enums.UnitStat, StatTemplate> _dataMap;

        public SerializableDictionary<Enums.UnitStat, StatTemplate> DataMap => _dataMap;
        
#if UNITY_EDITOR

        public static string StatsMapPropertyName => nameof(_dataMap);
        
#endif
    }
}