using Common.Battle.Interfaces;
using Core.Interfaces;

namespace Common.Battle.States
{
    public abstract class BattleStateBase : IBattleState
    {
        protected readonly IStateChanger<IBattleState> stateChanger;

        protected BattleStateBase(IStateChanger<IBattleState> stateChanger)
        {
            this.stateChanger = stateChanger;
        }

        public abstract void Activate();
    }
}