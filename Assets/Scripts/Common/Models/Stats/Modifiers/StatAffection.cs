using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Stats.Modifiers
{
    [Serializable]
    public struct StatAffection
    {
        [SerializeField] private Enums.UnitStat _affectedStat;
        [SerializeField] private Enums.StatModifierMultiplier _multiplier;
        [SerializeField] private float _value;

        public Enums.UnitStat AffectedStat => _affectedStat;
        public Enums.StatModifierMultiplier Multiplier => _multiplier;
        public float Value => _value;

#if UNITY_EDITOR
        public static string MultiplierPropertyName => nameof(_multiplier);
#endif
    }
}