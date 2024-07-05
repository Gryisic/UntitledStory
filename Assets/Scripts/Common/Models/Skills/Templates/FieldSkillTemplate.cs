using Common.Models.Skills.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Skills/Field Skill", fileName = "Field Skill")]
    public class FieldSkillTemplate : SkillTemplate, IFieldSkillData
    {
        [SerializeField] private Enums.FieldSkill _fieldSkill;

        public Enums.FieldSkill Skill => _fieldSkill;
        
        public override Enums.SkillType Type => Enums.SkillType.Field;
    }
}