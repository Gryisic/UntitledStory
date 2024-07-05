using System;
using Common.Units.Battle;
using Common.Units.Handlers;
using Common.Units.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.Turns
{
    public class TurnsHandler
    {
        private readonly BattleUnitsHandler _unitsHandler;
        private readonly PartyMemberTurn _partyMemberTurn;
        private readonly EnemyTurn _enemyTurn;

        private Turn _activeTurn;

        public TurnsHandler(BattleUnitsHandler unitsHandler)
        {
            _partyMemberTurn = new PartyMemberTurn();
            _enemyTurn = new EnemyTurn();

            _unitsHandler = unitsHandler;
        }

        public void ToNextTurn(out Enums.BattleTeam nextTeam)
        {
            _activeTurn?.Deactivate();

            if (_unitsHandler.GetNextAliveUnit() is IPartyMember)
            {
                _activeTurn = _partyMemberTurn;
                nextTeam = Enums.BattleTeam.Party;
            }
            else
            {
                _activeTurn = _enemyTurn;
                nextTeam = Enums.BattleTeam.Enemy;
            }
            
            _activeTurn.Activate();
        }
        
        public bool CanGoToNextTurn(out Enums.BattleTeam nextTeam)
        {
            nextTeam = Enums.BattleTeam.Party;
            
            if (_unitsHandler.HasUnitsWithFilter(u => u is IPartyMember && u.IsDead == false) == false)
            {
                nextTeam = Enums.BattleTeam.Enemy;
                return false;
            }

            if (_unitsHandler.HasUnitsWithFilter(u => u is BattleEnemy && u.IsDead == false) == false)
            {
                nextTeam = Enums.BattleTeam.Party;
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _activeTurn = null;
        }
    }
}