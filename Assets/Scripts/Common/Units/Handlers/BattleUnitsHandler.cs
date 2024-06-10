using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Battle;
using Common.Units.Extensions;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Handlers
{
    public class BattleUnitsHandler : UnitsHandler<BattleUnit>
    {
        private int _activeUnitIndex = -1;

        private readonly List<BattleUnit> _externalUnits;

        private List<BattleUnit> _inBattleUnits;
        
        public BattleUnit ActiveUnit { get; private set; }

        public IReadOnlyList<BattleUnit> Units
        {
            get
            {
                if (isDirty)
                {
                    _inBattleUnits = units.Cast<BattleUnit>().Concat(_externalUnits).ToList();
                    
                    Sort(ref _inBattleUnits);
                }

                return _inBattleUnits;
            }
        }

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
            isDirty = true;
        }

        public void Add(BattleUnit unit)
        {
            _externalUnits.Add(unit);

            isDirty = true;
        }

        private void Sort(ref List<BattleUnit> unitsToSort)
        {
            unitsToSort = unitsToSort
                .OrderByDescending(u => u.StatsHandler.GetStatData(Enums.UnitStat.Initiative).Value)
                .ToList();

            isDirty = false;
        }

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
            
            ActiveUnit = Units[_activeUnitIndex];

            return ActiveUnit;
        }

        public BattleUnit GetNextAliveUnit()
        {
            int step = 0;
            BattleUnit unit = GetNextUnit();
            
            while (unit.IsDead && step < _inBattleUnits.Count)
            {
                unit = GetNextUnit();
                
                step++;
            }

            return unit;
        }

        public bool HasUnitsWithFilter(Func<BattleUnit, bool> filter) => GetFilteredUnits(filter).Count > 0;

        public IReadOnlyList<BattleUnit> GetFilteredUnits(Func<BattleUnit, bool> filter) =>
            _inBattleUnits.Where(filter).ToList();

        public int GetNumberOfUnitsWithID(int id) => units.Count(u => u.ID == id);
    }
}