using System.Collections.Generic;
using System.Linq;
using Common.Units.Battle;
using Core.Extensions;

namespace Common.Units.Handlers
{
    public class BattleUnitsHandler : UnitsHandler<BattleUnit>
    {
        private readonly List<BattlePartyMember> _partyMembers;
        private readonly List<BattleEnemy> _enemies;

        private int _activeUnitIndex = -1;

        public BattleUnit ActiveUnit { get; private set; }
        
        public IReadOnlyList<BattleUnit> Units => units.Cast<BattleUnit>().ToList();

        public IReadOnlyList<BattlePartyMember> PartyMembers => _partyMembers;
        public IReadOnlyList<BattleEnemy> Enemies => _enemies;

        public BattleUnitsHandler()
        {
            _partyMembers = new List<BattlePartyMember>();
            _enemies = new List<BattleEnemy>();
        }

        public override void Add(Unit unit)
        {
            base.Add(unit);

            switch (unit)
            {
                case BattlePartyMember partyMember:
                    _partyMembers.Add(partyMember);
                    break;
                
                case BattleEnemy enemy:
                    _enemies.Add(enemy);
                    break;
            }
        }

        public override bool TryRemove(Unit unit)
        {
            bool isRemoved = base.TryRemove(unit);
            
            switch (unit)
            {
                case BattlePartyMember partyMember:
                    _partyMembers.Remove(partyMember);
                    break;
                
                case BattleEnemy enemy:
                    _enemies.Remove(enemy);
                    break;
            }
            
            return isRemoved;
        }

        public override void Clear()
        {
            base.Clear();
            
            _partyMembers.Clear();
            _enemies.Clear();

            _activeUnitIndex = -1;
            ActiveUnit = null;
        }

        public BattleUnit GetNextUnit()
        {
            _activeUnitIndex = _activeUnitIndex.Cycled(units.Count);

            ActiveUnit = units[_activeUnitIndex] as BattleUnit;

            return ActiveUnit;
        }
    }
}