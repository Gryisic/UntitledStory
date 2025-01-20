using System;
using System.Collections.Generic;
using Common.Models.Stats.Interfaces;
using UnityEngine;

namespace Common.Models.Stats.Modifiers
{
    [Serializable]
    public abstract class StatModifier : IStatModifier
    {
        [SerializeField] private int _id;
        [SerializeField] private StatAffection[] _affection;

        public int ID => _id;
        public IReadOnlyList<StatAffection> AffectedStats => _affection;
    }
}