using System;
using Common.Battle.Interfaces;
using Common.Battle.Turns;
using Common.Units.Handlers;
using Core.GameStates;
using Core.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.States
{
    public class TurnSelectionState : BattleStateBase, IResettable, IBattleStateArgsRequester, IBattleEndRequester
    {
        private readonly TurnsHandler _turnsHandler;
        private readonly BattleUnitsHandler _unitsHandler;
        
        public event Func<BattleStateArgs> RequestArgs;
        public event Action<Enums.BattleResult> RequestBattleEnd;

        public TurnSelectionState(IStateChanger<IBattleState> stateChanger, BattleUnitsHandler battleUnitsHandler) : base(stateChanger)
        {
            _unitsHandler = battleUnitsHandler;
            _turnsHandler = new TurnsHandler(battleUnitsHandler);
        }

        public override void Activate()
        {
            if (_turnsHandler.CanGoToNextTurn(out Enums.BattleTeam aliveTeam) == false)
            {
                OnOneTeamLeft(aliveTeam);
                
                return;
            }
            
            _turnsHandler.ToNextTurn(out Enums.BattleTeam nextTeam);
            
            BattleStateArgs args = RequestArgs?.Invoke();
            args.UpdateActiveUnitData(_unitsHandler.ActiveUnit);
            
            switch (nextTeam)
            {
                case Enums.BattleTeam.Party:
                    stateChanger.ChangeState<PartyTurnState>();
                    break;
                
                case Enums.BattleTeam.Enemy:
                    stateChanger.ChangeState<EnemyTurnState>();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Reset() => _turnsHandler.Reset();
        
        private void OnOneTeamLeft(Enums.BattleTeam team)
        {
            switch (team)
            {
                case Enums.BattleTeam.Party:
                    RequestBattleEnd?.Invoke(Enums.BattleResult.Win);
                    break;
                
                case Enums.BattleTeam.Enemy:
                    RequestBattleEnd?.Invoke(Enums.BattleResult.Lose);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }
    }
}