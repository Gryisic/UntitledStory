using System;
using Infrastructure.Utils;

namespace Common.Battle.Constraints
{
    [Serializable]
    public abstract class BattleConstraint 
    {
        public abstract Enums.BattleConstraint Constraint { get; }
    }
}