using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Interfaces;

namespace Core.GameStates
{
    public abstract class StatesChanger<T>: IStateChanger<T> where T: class, IChangeableState
    {
        protected IReadOnlyList<T> states;
        protected T currentState;

        public void ChangeState<TState>() where TState: T
        {
            currentState?.Deactivate();
            currentState = states.First(s => s is TState);
            currentState.Activate();
        }
    }
}