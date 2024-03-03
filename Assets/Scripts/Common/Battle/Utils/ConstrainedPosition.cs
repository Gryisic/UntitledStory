using System;
using Common.Units.Battle;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.Utils
{
    [Serializable]
    public class ConstrainedPosition
    {
        [SerializeField] private Transform _placementMarker;

        [SerializeField] private bool _hasBoundedUnit;
        [SerializeField] private int _id = Constants.IgnoredID;
        
        public Vector2 Position => _placementMarker.position.SnappedTo(Constants.BattleFieldHalfCellSize);
        
        public bool HasBoundedUnit => _hasBoundedUnit;
        public int ID => _id;
    }
}