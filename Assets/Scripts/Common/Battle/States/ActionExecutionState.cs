using Common.Battle.Interfaces;
using Common.QTE;
using Core.Interfaces;
using UnityEngine;

namespace Common.Battle.States
{
    public class ActionExecutionState : BattleStateBase
    {
        private QuickTimeEventExecutor _eventExecutor;

        public ActionExecutionState(IStateChanger<IBattleState> stateChanger) : base(stateChanger)
        {
            _eventExecutor = new QuickTimeEventExecutor();
        }

        public override void Activate()
        {
            Debug.Log("Action");
        }
    }
}