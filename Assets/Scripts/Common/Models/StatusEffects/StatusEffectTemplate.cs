using System.Collections.Generic;
using Common.Models.Stats.Modifiers;
using Common.Models.StatusEffects.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    [CreateAssetMenu(menuName = "Common/Templates/Status Effects/Effect")]
    public class StatusEffectTemplate : ScriptableObject, IStatusEffectData
    {
        [SerializeField, AsFileName] private string _effectName;
        [SerializeField] private int _id;
        [SerializeField] private Sprite _icon;

        [SerializeField] private Enums.StatusEffectType _type;
        [SerializeField] private Enums.Buff _buffType;
        [SerializeField] private Enums.Debuff _debuffType;

        [SerializeField] private Enums.StatusEffectExecute _execution;
        [SerializeField] private int _duration;

        [SerializeField] private StatAffection[] _affectedStats;

        public int ID => _id;
        public string Name => _effectName;
        public Sprite Icon => _icon;

        public Enums.StatusEffectType Type => _type;
        public Enums.Buff BuffType => _buffType;
        public Enums.Debuff DebuffType => _debuffType;

        public Enums.StatusEffectExecute Execution => _execution;
        public int Duration => _duration;

        public IReadOnlyList<StatAffection> AffectedStats => _affectedStats;
    }
}