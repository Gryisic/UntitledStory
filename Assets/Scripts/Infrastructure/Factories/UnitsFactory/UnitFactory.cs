﻿using System.Collections.Generic;
using Common.Models.Scene;
using Common.Units;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Factories.Extensions;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.UnitsFactory
{
    public class UnitFactory : IUnitFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Transform _root;
        
        private readonly Dictionary<int, IUnit> _idPrefabMap;
        
        public UnitFactory(DiContainer diContainer, SceneInfo sceneInfo)
        {
            _diContainer = diContainer;
            _root = sceneInfo.UnitsRoot;

            _idPrefabMap = new Dictionary<int, IUnit>();
        }

        public void Load(int id)
        {
            if (_idPrefabMap.ContainsKey(id))
                return;
            
            string name = id.DefineUnit();
            IUnit unit = Resources.Load<Unit>($"{Constants.PathToUnitPrefabs}/{name}");
            
            _idPrefabMap.Add(id, unit);
        }

        public void Load(IReadOnlyList<int> id)
        {
            foreach (var i in id) 
                Load(i);
        }

        public Unit Create(UnitTemplate template, Vector3 at)
        {
            Unit unit = _idPrefabMap[template.ID] as Unit;
            
            unit = _diContainer.InstantiatePrefabForComponent<Unit>(unit, at, Quaternion.identity, _root);
            
            return unit;
        }
    }
}