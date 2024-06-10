using System.Collections.Generic;
using Common.Models.BattleAction;
using Common.Models.Stats;
using UnityEngine;

namespace Common.Units.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Units/BattleUnit")]
    public class BattleUnitTemplate : UnitTemplate
    {
        [Space, Header("Battle Data")]
        [SerializeField] private BattleActionTemplate _basicAttackTemplate;
        [SerializeField] private BattleActionTemplate[] _skillsTemplates;

        [Space, Header("Stats Data")] 
        [SerializeField] private StatDataContainer _statDataContainer;
        
        public BattleActionTemplate BasicAttackTemplate => _basicAttackTemplate;
        public IEnumerable<BattleActionTemplate> SkillsTemplates => _skillsTemplates;
        
        public StatDataContainer StatDataContainer => _statDataContainer;

#if UNITY_EDITOR

        public static string StatDataContainerPropertyName => nameof(_statDataContainer);

#endif
    }
}