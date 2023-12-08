using Common.Battle.Interfaces;
using Core.Interfaces;

namespace Common.Battle.States
{
    public class BattleFinalizeState : BattleStateBase
    {
        public BattleFinalizeState(IStateChanger<IBattleState> stateChanger) : base(stateChanger) { }

        public override void Activate()
        {
            
        }
    }
}