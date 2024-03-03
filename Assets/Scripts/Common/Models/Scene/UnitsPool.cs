using System;
using System.Collections.Generic;
using System.Linq;
using Common.Units;
using Common.Units.Battle;
using Common.Units.Exploring;
using Common.Units.Extensions;
using Common.Units.Templates;
using Core.Configs.Interfaces;
using Core.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;

namespace Common.Models.Scene
{
    public class UnitsPool
    {
        private readonly IUnitFactory _unitFactory;
        private readonly IUnitsConfig _unitsConfig;
        
        private readonly SceneInfo _sceneInfo;
        private readonly List<Unit> _units;

        private IReadOnlyList<ExploringUnit> ExploringUnits => _units.Where(u => u is ExploringUnit).Cast<ExploringUnit>().ToList();
        
        private IReadOnlyList<BattleUnit> BattleUnits => _units.Where(u => u is BattleUnit).Cast<BattleUnit>().ToList();

        public UnitsPool(IUnitFactory unitFactory, IConfigsService configsService, SceneInfo sceneInfo)
        {
            _unitFactory = unitFactory;
            _unitsConfig = configsService.GetConfig<IUnitsConfig>();

            _sceneInfo = sceneInfo;
            
            _units = new List<Unit>();
        }

        public T GetUnitOfTypeWithID<T>(int id) where T : Unit
        {
            if (typeof(T) == typeof(BattleUnit))
                return GetUnitFrom<T>(BattleUnits, id);

            if (typeof(T) == typeof(ExploringUnit))
                return GetUnitFrom<T>(ExploringUnits, id);

            throw new InvalidOperationException($"Trying to get unit with incorrect type. Type {typeof(T)}");
        }

        public void Return(Unit unit) => _units.Add(unit);

        private T GetUnitFrom<T>(IReadOnlyList<Unit> units, int id) where T: Unit
        {
            Unit unit = units.FirstOrDefault(u => u.ID == id) as T;
            
            unit ??= CreateUnit(id);

            _units.Remove(unit);

            return (T) unit;
        }

        private Unit CreateUnit(int id)
        {
            UnitTemplate template = _unitsConfig.GetTemplateWithID(id);
            
            _unitFactory.Load(id);

            Unit unit = _unitFactory.Create(template, _sceneInfo.UnitsRoot.position);

            unit.DeactivateAndHide();
            _units.Add(unit);

            return unit;
        }
    }
}