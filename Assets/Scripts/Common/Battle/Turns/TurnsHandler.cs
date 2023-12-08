using Common.Units.Battle;
using Common.Units.Handlers;
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

        public void ToNextTurn(out Enums.BattleTurn nextTurn)
        {
            _activeTurn?.Deactivate();

            if (_unitsHandler.GetNextUnit() is BattlePartyMember)
            {
                _activeTurn = _partyMemberTurn;
                nextTurn = Enums.BattleTurn.Party;
            }
            else
            {
                _activeTurn = _enemyTurn;
                nextTurn = Enums.BattleTurn.Enemy;
            }
            
            _activeTurn.Activate();
        }

        public void Reset()
        {
            _activeTurn = null;
        }
    }
}