using System;
using System.Collections.Generic;
using Common.Units.Battle;
using Common.Units.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.TargetSelection
{
    public class TargetSelector
    {
        private IReadOnlyList<IBattleUnitSharedData> _possibleTargets;
        private IBattleUnitSharedData _selectedTarget;
        private int _targetIndex;
        private bool _isActive;

        public event Func<Type> RequestActiveUniteType; 
        public event Func<Type, IReadOnlyList<IBattleUnitSharedData>> RequestTargets; 
        public event Action<Transform> TargetUpdated;
        public event Action<Enums.TargetSelectionType> Activated;
        public event Action Deactivated;

        public void Activate(Enums.TargetSide side, Enums.TargetsQuantity quantity, Enums.TargetSelectionType selectionType)
        {
            _isActive = true;

            Type targetType = DefineTargetType(side);
            _possibleTargets = RequestTargets?.Invoke(targetType);
            _selectedTarget = _possibleTargets[_targetIndex];
            
            Activated?.Invoke(selectionType);
            TargetUpdated?.Invoke(_selectedTarget.Transform);
        }

        public void Deactivate()
        {
            Deactivated?.Invoke();
            
            _isActive = false;
        }

        public void Reset()
        {
            _targetIndex = 0;
            _selectedTarget = null;
        }

        public void MoveUp() => Move(Vector2.up);

        public void MoveDown() => Move(Vector2.down);
        
        public void MoveLeft() => Move(Vector2.left);

        public void MoveRight() => Move(Vector2.right);

        private void Move(Vector2 direction)
        {
            if (_isActive == false || _possibleTargets.Count <= 1)
                return;

            _targetIndex = direction.y < 0 ? _targetIndex.Cycled(_possibleTargets.Count) : _targetIndex.ReverseCycled(_possibleTargets.Count);
            _selectedTarget = _possibleTargets[_targetIndex];
            
            TargetUpdated?.Invoke(_selectedTarget.Transform);
        }

        private Type DefineTargetType(Enums.TargetSide side)
        {
            Type activeUnitType = RequestActiveUniteType?.Invoke();

            return side switch
            {
                Enums.TargetSide.SameAsUnit => activeUnitType,
                Enums.TargetSide.OppositeToUnit => activeUnitType == typeof(BattlePartyMember) ? typeof(BattleEnemy) : typeof(BattlePartyMember),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
    }
}