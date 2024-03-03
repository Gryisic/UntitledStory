using Core.Utils.ObservableVariables.Interfaces;

namespace Common.Battle.Interfaces
{
    public interface IBattleData
    {
        IObservableVariable<int> CurrentTurn { get; }
    }
}