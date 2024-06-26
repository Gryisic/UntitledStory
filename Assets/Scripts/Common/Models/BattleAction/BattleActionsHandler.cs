﻿using System.Collections.Generic;
using System.Linq;
using Common.Models.BattleAction.Interfaces;
using Common.Models.Stats.Interfaces;

namespace Common.Models.BattleAction
{
    public class BattleActionsHandler
    {
        private readonly BattleAction _basicAttack;
        private readonly List<BattleAction> _actions;

        public IReadOnlyList<IBattleActionData> ExposedData => _actions.Select(a => a.Data).ToList();

        public BattleActionsHandler(BattleActionTemplate basicAttackTemplate, IEnumerable<BattleActionTemplate> skillTemplates, IStatsHandler stats)
        {
            _basicAttack = new BattleAction(basicAttackTemplate, stats);
            
            _actions = new List<BattleAction>();

            foreach (var template in skillTemplates)
            {
                BattleAction action = new BattleAction(template, stats);
                
                _actions.Add(action);
            }
        }

        public BattleAction GetBasicAttack() => _basicAttack;
        
        public BattleAction GetAction(int index) => _actions[index];
    }
}