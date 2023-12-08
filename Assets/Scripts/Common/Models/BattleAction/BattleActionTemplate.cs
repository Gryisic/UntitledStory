using System.Collections.Generic;
using Common.Models.BattleAction.Effects;
using Common.Models.BattleAction.Interfaces;
using Common.QTE.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.BattleAction
{
    [CreateAssetMenu(menuName = "Common/Templates/BattleActions/Action", fileName = "Action")]
    public class BattleActionTemplate : ScriptableObject, IBattleActionData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;

        [SerializeField] private int _cost;

        [SerializeField] private Enums.TargetSide _targetTeam;
        [SerializeField] private Enums.TargetsQuantity _targetsQuantity;

        [SerializeField] private QuickTimeEventTemplate[] _quickTimeEventTemplates;

        [SerializeReference, SubclassesPicker] private List<BattleActionEffect> _effects;

        public Sprite Icon => _icon;
        public string Name => _name;
        public int Cost => _cost;
        public string Description => _description;
        
        public Enums.TargetSide TargetTeam => _targetTeam;
        public Enums.TargetsQuantity TargetsQuantity => _targetsQuantity;

        public QuickTimeEventTemplate[] QuickTimeEventTemplates => _quickTimeEventTemplates;

        public IEnumerable<BattleActionEffect> Effects => _effects;
    }
}