using System;
using System.Collections.Generic;
using Common.Models.Animator;
using Common.Models.BattleAction;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Units.Battle
{
    public abstract class BattleUnit : Unit, IBattleUnitSharedData
    {
        public BattleActionsHandler ActionsHandler { get; private set; }
        
        public override void Initialize(UnitTemplate template)
        {
            if (template is BattleUnitTemplate battleUnitTemplate == false)
                throw new InvalidOperationException($"Trying to initialize exploring unit via non exploring temaplate. Template: {template}");

            CustomAnimator animator = new CustomAnimator(localRenderer);

            InitializeActionsHandler(battleUnitTemplate);

            internalData = new UnitInternalData(template, animator, transform, localRigidbody);
            actionsExecutor = new UnitActionsExecutor();

            base.Initialize(template);
        }

        public void MoveToPoint(Vector3 point) => actionsExecutor.AddActionToQueue(new MoveToPointAction(internalData, point, 5));

        public void ExecuteActions() => actionsExecutor.Execute();

        public async UniTask ExecuteActionsWithAwaiter() => await actionsExecutor.ExecuteWithAwaiter();
        
        private void InitializeActionsHandler(BattleUnitTemplate battleUnitTemplate)
        {
            IEnumerable<BattleActionTemplate> actionTemplates = battleUnitTemplate.SkillsTemplates;

            ActionsHandler = new BattleActionsHandler(battleUnitTemplate.BasicAttackTemplate, actionTemplates);
        }
    }
}