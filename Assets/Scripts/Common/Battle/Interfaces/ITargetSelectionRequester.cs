using System;
using Common.Battle.TargetSelection.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.Interfaces
{
    public interface ITargetSelectionRequester
    {
        event Action<ITargetSelectionData> RequestTargetSelection;
        event Action SuppressTargetSelection;
    }
}