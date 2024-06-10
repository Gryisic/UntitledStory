using Common.Models.BattleAction;
using Common.Models.BattleAction.Interfaces;
using Common.Models.Stats;
using Common.Models.Stats.Interfaces;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IBattleUnitSharedData : IUnitSharedData
    {
        BattleActionsHandler ActionsHandler { get; }
        IStatsHandler StatsHandler { get; }
        Transform Transform { get; }
    }
}