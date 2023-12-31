﻿using System;
using System.Collections.Generic;
using Common.Units.Battle;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.Constraints
{
    public class ExternalUnitsConstraint : BattleConstraint
    {
        [SerializeField] private UnitTemplateMap[] _units;

        private Dictionary<BattleUnit, BattleUnitTemplate> _unitsMap;

        public override Enums.BattleConstraint Constraint => Enums.BattleConstraint.ExternalUnits;
        public override int Priority => 999;

        public IReadOnlyDictionary<BattleUnit, BattleUnitTemplate> UnitsMap
        {
            get
            {
                if (_unitsMap == null) 
                    InitializeUnitsMap();

                return _unitsMap;
            }
        }

        private void InitializeUnitsMap()
        {
            _unitsMap = new Dictionary<BattleUnit, BattleUnitTemplate>();

            foreach (var map in _units)
                _unitsMap.Add(map.Unit, map.Template);
        }

        [Serializable]
        private class UnitTemplateMap
        {
            [SerializeField] private BattleUnit _unit;
            [SerializeField] private BattleUnitTemplate _template;

            public BattleUnit Unit => _unit;
            public BattleUnitTemplate Template => _template;
        }
    }
}