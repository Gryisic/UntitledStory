using Common.Models.Skills.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Skills/Passive Skill", fileName = "Passive Skill")]
    public class PassiveSkillTemplate : SkillTemplate, IPassiveSkillData
    {
        public override Enums.SkillType Type => Enums.SkillType.Passive;
    }
}