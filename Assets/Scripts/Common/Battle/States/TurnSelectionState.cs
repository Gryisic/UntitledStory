using System;
using Common.Battle.Interfaces;
using Common.Battle.Turns;
using Common.Units.Handlers;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.States
{
    public class TurnSelectionState : BattleStateBase, IResettable
    {
        private readonly TurnsHandler _turnsHandler;

        public TurnSelectionState(IStateChanger<IBattleState> stateChanger, BattleUnitsHandler battleUnitsHandler) : base(stateChanger)
        {
            _turnsHandler = new TurnsHandler(battleUnitsHandler);
        }

        public override void Activate()
        {
            _turnsHandler.ToNextTurn(out Enums.BattleTurn nextTurn);

            switch (nextTurn)
            {
                case Enums.BattleTurn.Party:
                    stateChanger.ChangeState<PartyTurnState>();
                    break;
                
                case Enums.BattleTurn.Enemy:
                    stateChanger.ChangeState<EnemyTurnState>();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset() => _turnsHandler.Reset();
    }
}