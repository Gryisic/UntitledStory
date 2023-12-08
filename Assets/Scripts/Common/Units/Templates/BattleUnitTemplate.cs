using System.Collections.Generic;
using Common.Models.BattleAction;
using UnityEngine;

namespace Common.Units.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Units/BattleUnit")]
    public class BattleUnitTemplate : UnitTemplate
    {
        [SerializeField] private BattleActionTemplate _basicAttackTemplate;
        [SerializeField] private BattleActionTemplate[] _skillsTemplates;

        public BattleActionTemplate BasicAttackTemplate => _basicAttackTemplate;
        public IEnumerable<BattleActionTemplate> SkillsTemplates => _skillsTemplates;
    }
}