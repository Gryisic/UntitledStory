using System;
using Common.Battle.Interfaces;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IBattleEvent
    {
        event Func<IBattleData> RequestBattleData;
    }
}