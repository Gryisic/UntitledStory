using Infrastructure.Utils;

namespace Common.Battle.TargetSelection.Interfaces
{
    public interface ITargetSelectionData
    {
        Enums.TargetSide Side { get; }
        Enums.TargetsQuantity Quantity { get; } 
        Enums.TargetSelectionFilter SelectionFilter { get; }
    }
}