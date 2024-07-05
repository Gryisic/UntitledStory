using System.Collections.Generic;
using Common.Models.Skills.Templates;
using Common.Models.Stats;
using UnityEngine;

namespace Common.Units.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Units/BattleUnit")]
    public class BattleUnitTemplate : UnitTemplate
    {
        [Space, Header("Battle Data")]
        [SerializeField] private SkillTemplate _basicAttackTemplate;
        [SerializeField] private SkillTemplate[] _skillsTemplates;

        [Space, Header("Stats Data")] 
        [SerializeField] private StatDataContainer _statDataContainer;
        
        public SkillTemplate BasicAttackTemplate => _basicAttackTemplate;
        public IEnumerable<SkillTemplate> SkillsTemplates => _skillsTemplates;
        
        public StatDataContainer StatDataContainer => _statDataContainer;

#if UNITY_EDITOR

        public static string StatDataContainerPropertyName => nameof(_statDataContainer);

#endif
    }
}