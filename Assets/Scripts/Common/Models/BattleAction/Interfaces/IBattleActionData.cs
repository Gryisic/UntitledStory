using Common.UI.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.BattleAction.Interfaces
{
    public interface IBattleActionData : IListedItemData
    {
        public Enums.TargetSide TargetTeam { get; }
        public Enums.TargetsQuantity TargetsQuantity { get; }
    }
}