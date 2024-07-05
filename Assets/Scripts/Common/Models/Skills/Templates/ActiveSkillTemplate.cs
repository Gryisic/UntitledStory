using Common.Models.Skills.Interfaces;
using Common.QTE;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/Skills/Active Skill", fileName = "Active Skill")]
    public class ActiveSkillTemplate : SkillTemplate, IActiveSkillData
    {
        [SerializeField] private int _cost;

        [SerializeField] private Enums.TargetSide _targetTeam;
        [SerializeField] private Enums.TargetsQuantity _targetsQuantity;
        [SerializeField] private Enums.TargetSelectionFilter _filter;
        
        [SerializeField] private bool _hasQTE = true;

        [SerializeField] private QuickTimeEventSequence _quickTimeEventSequence;
        
        public override Enums.SkillType Type => Enums.SkillType.Active;
        
        public int Cost => _cost;
        
        public Enums.TargetSide Side => _targetTeam;
        public Enums.TargetsQuantity Quantity => _targetsQuantity;
        public Enums.TargetSelectionFilter SelectionFilter => _filter;
        
        public bool HasQTE => _hasQTE;
        public QuickTimeEventSequence QuickTimeEventSequence => _quickTimeEventSequence;
        
#if UNITY_EDITOR
        public static string QTESequencePropertyName => nameof(_quickTimeEventSequence);
#endif
    }
}