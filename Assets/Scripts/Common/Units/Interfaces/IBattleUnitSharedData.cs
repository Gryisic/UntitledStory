using Common.Models.BattleAction;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IBattleUnitSharedData : IUnitSharedData
    {
        BattleActionsHandler ActionsHandler { get; }
        Transform Transform { get; }
    }
}