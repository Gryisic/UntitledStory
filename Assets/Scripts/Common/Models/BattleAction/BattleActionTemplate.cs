using System.Collections.Generic;
using Common.Models.BattleAction.Effects;
using Common.Models.BattleAction.Interfaces;
using Common.QTE;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
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
        [SerializeField] private Enums.TargetSelectionFilter _filter;

        [SerializeReference, SubclassesPicker] private BattleActionEffect[] _effects;

        [SerializeField] private bool _hasQTE = true;

        [SerializeField] private QuickTimeEventSequence _quickTimeEventSequence;

        public Sprite Icon => _icon;
        public string Name => _name;
        public int Cost => _cost;
        public string Description => _description;

        public Enums.TargetSide Side => _targetTeam;
        public Enums.TargetsQuantity Quantity => _targetsQuantity;
        public Enums.TargetSelectionFilter SelectionFilter => _filter;
        
        public bool HasQTE => _hasQTE;
        public QuickTimeEventSequence QuickTimeEventSequence => _quickTimeEventSequence;

        public IEnumerable<BattleActionEffect> Effects => _effects;

#if UNITY_EDITOR
        public static string QTESequencePropertyName => nameof(_quickTimeEventSequence);
#endif
    }
}