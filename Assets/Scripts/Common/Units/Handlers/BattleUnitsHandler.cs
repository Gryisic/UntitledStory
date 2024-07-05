using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Battle;
using Common.Units.Extensions;
using Common.Units.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Handlers
{
    public class BattleUnitsHandler : UnitsHandler<IBattleUnit>
    {
        private int _activeUnitIndex = -1;

        private readonly List<IBattleUnit> _externalUnits;

        private List<IBattleUnit> _inBattleUnits;
        
        public IBattleUnit ActiveUnit { get; private set; }

        public IReadOnlyList<IBattleUnit> Units
        {
            get
            {
                if (isDirty)
                {
                    _inBattleUnits = units.Cast<IBattleUnit>().Concat(_externalUnits).ToList();
                    
                    Sort(ref _inBattleUnits);
                }

                return _inBattleUnits;
            }
        }

        public IReadOnlyList<IPartyMember> PartyMembers => Units.Where(u => u is IPartyMember).Cast<IPartyMember>().ToList();
        public IReadOnlyList<IBattleEnemy> Enemies => Units.Where(u => u is IBattleEnemy).Cast<IBattleEnemy>().ToList();

        public BattleUnitsHandler(UnitsPool pool) : base(pool)
        {
            _externalUnits = new List<IBattleUnit>();
        }

        public override void Clear()
        {
            base.Clear();
            
            _externalUnits.Clear();

            _activeUnitIndex = -1;
            ActiveUnit = null;
            isDirty = true;
        }

        public void Add(IBattleUnit unit)
        {
            _externalUnits.Add(unit);

            isDirty = true;
        }

        private void Sort(ref List<IBattleUnit> unitsToSort)
        {
            unitsToSort = unitsToSort
                .OrderByDescending(u => u.StatsHandler.GetStatData(Enums.UnitStat.Initiative).Value)
                .ToList();

            isDirty = false;
        }

        public IReadOnlyList<IBattleUnit> GetUnitsOfType(Type type)
        {
            if (type == typeof(IPartyMember))
                return PartyMembers;
            
            if (type == typeof(IBattleEnemy))
                return Enemies;

            throw new InvalidOperationException($"Trying to get units of invalid type. Type: {type}");
        }

        public IBattleUnit GetNextUnit()
        {
            _activeUnitIndex = _activeUnitIndex.Cycled(Units.Count);
            
            ActiveUnit = Units[_activeUnitIndex];

            return ActiveUnit;
        }

        public IBattleUnit GetNextAliveUnit()
        {
            int step = 0;
            IBattleUnit unit = GetNextUnit();
            
            while (unit.IsDead && step < _inBattleUnits.Count)
            {
                unit = GetNextUnit();
                
                step++;
            }

            return unit;
        }

        public bool HasUnitsWithFilter(Func<IBattleUnit, bool> filter) => GetFilteredUnits(filter).Count > 0;

        public IReadOnlyList<IBattleUnit> GetFilteredUnits(Func<IBattleUnit, bool> filter) =>
            _inBattleUnits.Where(filter).ToList();

        public int GetNumberOfUnitsWithID(int id) => units.Count(u => u.ID == id);
    }
}