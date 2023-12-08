using Common.Battle.Interfaces;
using Core.Interfaces;

namespace Common.Battle.States
{
    public class EnemyTurnState : BattleStateBase
    {
        public EnemyTurnState(IStateChanger<IBattleState> stateChanger) : base(stateChanger) { }

        public override void Activate()
        {
            
        }
    }
}