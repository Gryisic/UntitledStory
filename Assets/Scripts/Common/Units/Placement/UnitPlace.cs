using System;
using Common.Units.Placement.Interfaces;
using UnityEngine;

namespace Common.Units.Placement
{
    [Serializable]
    public struct UnitPlace : IUnitPlacementData
    {
        [SerializeField] private int _id;
        [SerializeField] private Transform _positionMarker;

        public int ID => _id;
        public Vector2 Position => _positionMarker.position;
    }
}