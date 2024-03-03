using System;
using Infrastructure.Utils;

namespace Common.Battle.Interfaces
{
    public interface ITargetSelectionRequester
    {
        event Action<Enums.TargetSide, Enums.TargetsQuantity, Enums.TargetSelectionType> RequestTargetSelection;
        event Action SuppressTargetSelection;
    }
}