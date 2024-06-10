using System;
using Infrastructure.Utils;

namespace Common.Battle.Interfaces
{
    public interface IBattleEndRequester
    {
        event Action<Enums.BattleResult> RequestBattleEnd;
    }
}