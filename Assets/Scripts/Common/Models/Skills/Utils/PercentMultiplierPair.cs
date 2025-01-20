using System;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Skills.Utils
{
    [Serializable]
    public struct PercentMultiplierPair
    {
        [SerializeField] private PercentComparator _comparator;
        [SerializeField] private float _multiplier;

        public float Multiplier => _multiplier;

        public bool IsInRange(float value) => _comparator.IsInRange((int) value);
    }
}