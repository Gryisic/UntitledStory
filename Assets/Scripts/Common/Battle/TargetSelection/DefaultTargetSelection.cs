using Common.Battle.TargetSelection.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.TargetSelection
{
    public class DefaultTargetSelection : ITargetSelectionData
    {
        public Enums.TargetSide Side => Enums.TargetSide.OppositeToUnit;
        public Enums.TargetsQuantity Quantity => Enums.TargetsQuantity.Single;
        public Enums.TargetSelectionFilter SelectionFilter => Enums.TargetSelectionFilter.Alive;
    }
}