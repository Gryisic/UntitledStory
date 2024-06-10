using System;
using Common.Models.StatusEffects.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    [CreateAssetMenu(menuName = "Common/Templates/StatusEffects/Effect")]
    public class StatusEffectTemplate : ScriptableObject, IStatusEffectData
    {
        [SerializeField, AsFileName] private string _effectName;
        [SerializeField] private Sprite _icon;

        [SerializeField] private Enums.StatusEffectType _type;
        [SerializeField] private Enums.Buff _buffType;
        [SerializeField] private Enums.Debuff _debuffType;

        [SerializeField] private StatAffection[] _affectedStats;

        public string Name => _effectName;
        
        [Serializable]
        private struct StatAffection
        {
            [SerializeField] private Enums.UnitStat _affectedStat;
            [SerializeField] private Enums.PresentedValueType _valueType;
            [SerializeField] private float _value;

            public Enums.UnitStat AffectedStat => _affectedStat;
            public Enums.PresentedValueType ValueType => _valueType;
            public float Value => _value;
        }
    }
}