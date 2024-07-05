using System.Collections.Generic;
using Common.Models.Skills.Effects;
using Common.Models.Skills.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Skills.Templates
{
    public abstract class SkillTemplate : ScriptableObject, ISkillData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;

        [SerializeReference, SubclassesPicker] private SkillEffect[] _effects;

        public abstract Enums.SkillType Type { get; }
        
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;

        public IEnumerable<SkillEffect> Effects => _effects;
    }
}