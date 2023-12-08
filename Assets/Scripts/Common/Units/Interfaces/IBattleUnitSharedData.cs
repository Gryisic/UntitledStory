using Common.Models.BattleAction;

namespace Common.Units.Interfaces
{
    public interface IBattleUnitSharedData : IUnitSharedData
    {
        BattleActionsHandler ActionsHandler { get; }
    }
}