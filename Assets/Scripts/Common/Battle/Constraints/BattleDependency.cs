using System;
using Common.Models.Triggers.Dependencies;
using Infrastructure.Utils;

namespace Common.Battle.Constraints
{
    [Serializable]
    public abstract class BattleDependency : Dependency
    {
        public abstract Enums.BattleConstraint Constraint { get; }
    }
}