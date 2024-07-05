using System;
using Common.Models.Animator;
using Common.Units.Actions;
using Common.Units.Templates;
using Infrastructure.Utils;

namespace Common.Units.Exploring
{
    public abstract class ExploringUnit : Unit
    {
        public override void Initialize(UnitTemplate template)
        {
            if (template is ExploringUnitTemplate == false)
                throw new InvalidOperationException($"Trying to initialize exploring unit via non exploring temaplate. Template: {template}");

            CustomAnimator animator = new CustomAnimator(localRenderer);
            
            internalData = new UnitInternalData(template, animator, transform, localRigidbody);
            actionsExecutor = new UnitActionsExecutor();
            
            base.Initialize(template);
        }

        public override void Dispose()
        {
            if (isActive)
                Deactivate();
            
            actionsExecutor.Dispose();
            
            base.Dispose();
        }

        public override void Activate()
        {
            base.Activate();
            
            internalData.Animator.PlayCyclic(internalData.Data.GetAnimation(Enums.StandardAnimation.Idle));
        }

        public void CancelActions() => actionsExecutor.CancelAllActions();
    }
}