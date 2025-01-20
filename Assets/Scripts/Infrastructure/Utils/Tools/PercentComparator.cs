using System;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Infrastructure.Utils.Tools
{
    [Serializable]
    public struct PercentComparator
    {
        [SerializeField, Range(0, 100)] private int _percentToCompare;
        [SerializeField, RenameLabel("Comparison Type")] private Enums.ValueComparator _valueComparator;

        public bool IsInRange(int value)
        {
            return _valueComparator switch
            {
                Enums.ValueComparator.Equals => value == _percentToCompare,
                Enums.ValueComparator.Greater => value > _percentToCompare,
                Enums.ValueComparator.GreaterOrEquals => value >= _percentToCompare,
                Enums.ValueComparator.Less => value < _percentToCompare,
                Enums.ValueComparator.LessOrEquals => value <= _percentToCompare,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}