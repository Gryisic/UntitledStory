using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Battle;
using Common.Units.Extensions;
using Core.Extensions;

namespace Common.Units.Handlers
{
    public class BattleUnitsHandler : UnitsHandler<BattleUnit>
    {
        private int _activeUnitIndex = -1;

        private readonly List<BattleUnit> _externalUnits;

        public BattleUnit ActiveUnit { get; private set; }
        
        public IReadOnlyList<BattleUnit> Units => units.Cast<BattleUnit>().Concat(_externalUnits).ToList();

        public IReadOnlyList<BattlePartyMember> PartyMembers => Units.Where(u => u is BattlePartyMember).Cast<BattlePartyMember>().ToList();
        public IReadOnlyList<BattleEnemy> Enemies => Units.Where(u => u is BattleEnemy).Cast<BattleEnemy>().ToList();

        public BattleUnitsHandler(UnitsPool pool) : base(pool)
        {
            _externalUnits = new List<BattleUnit>();
        }

        public override void Clear()
        {
            base.Clear();
            
            _externalUnits.Clear();

            _activeUnitIndex = -1;
            ActiveUnit = null;
        }

        public void Add(BattleUnit unit) => _externalUnits.Add(unit);

        public IReadOnlyList<BattleUnit> GetUnitsOfType(Type type)
        {
            if (type == typeof(BattleEnemy))
                return Enemies;
            
            if (type == typeof(BattlePartyMember))
                return PartyMembers;

            throw new InvalidOperationException($"Trying to get units of invalid type. Type: {type}");
        }

        public BattleUnit GetNextUnit()
        {
            _activeUnitIndex = _activeUnitIndex.Cycled(Units.Count);

            ActiveUnit = Units[_activeUnitIndex] as BattleUnit;

            return ActiveUnit;
        }
        
        public int GetNumberOfUnitsWithID(int id) => units.Count(u => u.ID == id);
    }
}