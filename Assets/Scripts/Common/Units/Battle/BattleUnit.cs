using System;
using System.Collections.Generic;
using Common.Models.Animator;
using Common.Models.Impactable.Interfaces;
using Common.Models.Skills;
using Common.Models.Skills.Templates;
using Common.Models.Stats;
using Common.Models.Stats.Interfaces;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Battle
{
    public abstract class BattleUnit : Unit, IBattleUnit
    {
        public SkillsHandler SkillsHandler { get; private set; }
        public IStatsHandler StatsHandler { get; private set; }
        
        public bool IsDead { get; private set; }
        
        public event Action<IImpactable, int> AppliedDamaged;
        public event Action<IImpactable, int> Healed;
        public event Action<IImpactable, IStatusEffectData> AppliedStatusEffect;

        public override void Initialize(UnitTemplate template)
        {
            if (template is BattleUnitTemplate battleUnitTemplate == false)
                throw new InvalidOperationException($"Trying to initialize exploring unit via non exploring temaplate. Template: {template}");

            CustomAnimator animator = new CustomAnimator(localRenderer);

            InitializeStatsHandler(battleUnitTemplate);
            InitializeActionsHandler(battleUnitTemplate);

            internalData = new UnitInternalData(template, animator, transform, localRigidbody);
            actionsExecutor = new UnitActionsExecutor();

            base.Initialize(template);
        }

        public void ApplyDamage(int amount)
        {
            StatsHandler.DecreaseStat(Enums.UnitStat.Health, amount);
            
            AppliedDamaged?.Invoke(this, amount);
            
            if (StatsHandler.GetStatData(Enums.UnitStat.Health).Value <= 0)
                Die();
        }

        public void Heal(int amount)
        {
            StatsHandler.IncreaseStat(Enums.UnitStat.Health, amount);
            
            Healed?.Invoke(this, amount);
        }
        
        public void ApplyStatusEffect(IStatusEffect effect)
        {
            //AppliedStatusEffect?.Invoke(this, );
        }
        
        public virtual void Select() { }

        public virtual void Deselect() { }
        
        public void MoveToPoint(Vector3 point) => actionsExecutor.AddActionToQueue(new MoveToPointAction(internalData, point, 5));

        public void MoveToPointAndLookAt(Vector3 point, Vector2 lookDirection) => actionsExecutor
                .AddActionToQueue(new MoveToPointAction(internalData, point, 5))
                .AddCallback(() => internalData.Flip(lookDirection));

        public void ExecuteActions() => actionsExecutor.Execute();

        public async UniTask ExecuteActionsWithAwaiter() => await actionsExecutor.ExecuteWithAwaiter();

        public void Restore()
        {
            int maxHealth = StatsHandler.GetStatData(Enums.UnitStat.MaxHealth).Value;
            int health = StatsHandler.GetStatData(Enums.UnitStat.Health).Value;
            int healAmount = maxHealth - health;
            
            Heal(healAmount);

            IsDead = false;
        }

        protected void Die()
        {
            Debug.Log($"{name}: Well, looks like I'm dead!");

            IsDead = true;
        }

        private void InitializeActionsHandler(BattleUnitTemplate battleUnitTemplate)
        {
            IEnumerable<SkillTemplate> actionTemplates = battleUnitTemplate.SkillsTemplates;

            SkillsHandler = new SkillsHandler(battleUnitTemplate.BasicAttackTemplate, actionTemplates, StatsHandler);
        }
        
        private void InitializeStatsHandler(BattleUnitTemplate battleUnitTemplate) 
            => StatsHandler = new StatsHandler(battleUnitTemplate.StatDataContainer);
    }
}