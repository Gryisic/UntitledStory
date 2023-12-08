using System.Collections.Generic;
using System.Linq;
using Common.Models.BattleAction.Interfaces;

namespace Common.Models.BattleAction
{
    public class BattleActionsHandler
    {
        private readonly BattleAction _basicAttack;
        private readonly List<BattleAction> _actions;

        public IEnumerable<IBattleActionData> ExposedData => _actions.Select(a => a.Data);

        public BattleActionsHandler(BattleActionTemplate basicAttackTemplate, IEnumerable<BattleActionTemplate> skillTemplates)
        {
            _basicAttack = new BattleAction(basicAttackTemplate);
            
            _actions = new List<BattleAction>();

            foreach (var template in skillTemplates)
            {
                BattleAction action = new BattleAction(template);
                
                _actions.Add(action);
            }
        }

        public BattleAction GetBasicAttack() => _basicAttack;
        
        public BattleAction GetAction(int index) => _actions[index];
    }
}