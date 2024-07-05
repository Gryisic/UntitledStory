using System;
using System.Collections.Generic;
using System.Linq;
using Common.Units;
using Common.Units.Battle;
using Common.Units.Exploring;
using Common.Units.Extensions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Core.Configs.Interfaces;
using Core.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using UnityEngine;

namespace Common.Models.Scene
{
    public class UnitsPool
    {
        private readonly IUnitFactory _unitFactory;
        private readonly IUnitsConfig _unitsConfig;
        
        private readonly SceneInfo _sceneInfo;
        private readonly List<IUnit> _units;

        private IReadOnlyList<IExploringUnit> ExploringUnits => _units.Where(u => u is IExploringUnit).Cast<IExploringUnit>().ToList();
        
        private IReadOnlyList<IBattleUnit> BattleUnits => _units.Where(u => u is IBattleUnit).Cast<IBattleUnit>().ToList();

        public UnitsPool(IUnitFactory unitFactory, IConfigsService configsService, SceneInfo sceneInfo)
        {
            _unitFactory = unitFactory;
            _unitsConfig = configsService.GetConfig<IUnitsConfig>();

            _sceneInfo = sceneInfo;
            
            _units = new List<IUnit>();
        }

        public T GetUnitOfTypeWithID<T>(int id) where T : class, IUnit
        {
            if (typeof(T) == typeof(BattleUnit))
                return GetUnitFrom<T>(BattleUnits, id);

            if (typeof(T) == typeof(ExploringUnit))
                return GetUnitFrom<T>(ExploringUnits, id);

            return GetUnitFrom<T>(_units, id);
        }

        public void Return(IUnit unit) => _units.Add(unit);

        private T GetUnitFrom<T>(IReadOnlyList<IUnit> units, int id) where T: class, IUnit
        {
            IUnit unit = units.FirstOrDefault(u => u.ID == id) as T;
            
            unit ??= CreateUnit(id);

            _units.Remove(unit);
            
            return (T) unit;
        }

        private Unit CreateUnit(int id)
        {
            UnitTemplate template = _unitsConfig.GetTemplateWithID(id);
            
            _unitFactory.Load(id);

            Unit unit = _unitFactory.Create(template, _sceneInfo.UnitsRoot.position);

            unit.Initialize(template);
            unit.DeactivateAndHide();
            _units.Add(unit);

            return unit;
        }
    }
}