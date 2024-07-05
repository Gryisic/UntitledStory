using System;
using System.Collections.Generic;
using System.Linq;
using Common.Battle.TargetSelection.Interfaces;
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
        public event Action<IBattleUnitSharedData> TargetUpdated;
        public event Action Activated;
        public event Action Deactivated;

        public void Activate(ITargetSelectionData data)
        {
            _isActive = true;

            Type targetType = DefineTargetType(data.Side);
            
            _possibleTargets = RequestTargets?.Invoke(targetType);
            _possibleTargets = GetFilteredTargets(data.SelectionFilter);
            
            if (_possibleTargets.Count <= 0)
                return;
            
            _targetIndex = _targetIndex >= _possibleTargets.Count ? 0 : _targetIndex;
            _selectedTarget = _possibleTargets[_targetIndex];
            
            _selectedTarget.Select();
            
            Activated?.Invoke();
            TargetUpdated?.Invoke(_selectedTarget);
        }

        public void Deactivate()
        {
            _selectedTarget.Deselect();
            
            Deactivated?.Invoke();
            
            _isActive = false;
        }

        public void Reset()
        {
            _targetIndex = 0;
            
            _selectedTarget.Deselect();
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

            _selectedTarget.Deselect();
            
            _targetIndex = direction.y < 0 ? _targetIndex.Cycled(_possibleTargets.Count) : _targetIndex.ReverseCycled(_possibleTargets.Count);
            _selectedTarget = _possibleTargets[_targetIndex];
            
            _selectedTarget.Select();
            
            TargetUpdated?.Invoke(_selectedTarget);
        }

        private IReadOnlyList<IBattleUnitSharedData> GetFilteredTargets(Enums.TargetSelectionFilter filter)
        {
            switch (filter)
            {
                case Enums.TargetSelectionFilter.All:
                    return _possibleTargets;
                
                case Enums.TargetSelectionFilter.Alive:
                    return _possibleTargets.Where(t => t.IsDead == false).ToList();
                
                case Enums.TargetSelectionFilter.Dead:
                    return _possibleTargets.Where(t => t.IsDead).ToList();
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        private Type DefineTargetType(Enums.TargetSide side)
        {
            Type activeUnitType = RequestActiveUniteType?.Invoke();
            
            return side switch
            {
                Enums.TargetSide.SameAsUnit => activeUnitType,
                Enums.TargetSide.OppositeToUnit => activeUnitType.GetInterface(nameof(IPartyMember)) != null ? typeof(IBattleEnemy) : typeof(IPartyMember),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
    }
}