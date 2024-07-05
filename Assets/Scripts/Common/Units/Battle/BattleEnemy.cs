using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Common.UI.Enemy;
using Common.Units.Behaviour;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Battle
{
    public class BattleEnemy : BattleUnit, IBattleEnemy
    {
        [SerializeField] private BehaviourProcessor _behaviourProcessor;
        [SerializeField] private EnemyHealthBarView _healthBar;

        public override void Initialize(UnitTemplate template)
        {
            _behaviourProcessor.Initialize();
            
            base.Initialize(template);
        }
        
        public override void Dispose()
        {
            Deactivate();
            
            _behaviourProcessor.Dispose();
            
            base.Dispose();
        }

        public override void Activate()
        {
            AppliedDamaged += OnHealthAffected;
            Healed += OnHealthAffected;
            
            _behaviourProcessor.Start();
            
            base.Activate();
        }

        public override void Deactivate()
        {
            AppliedDamaged -= OnHealthAffected;
            Healed -= OnHealthAffected;
            
            base.Deactivate();
        }

        public override void Select()
        {
            base.Select();
            
            _healthBar.Activate();
        }

        public override void Deselect()
        {
            _healthBar.Deactivate();
            
            base.Deselect();
        }
        
        private void OnHealthAffected(IImpactable impactable, int damage)
        {
            StatsHandler.GetHealthData(out IStatData currentHealth, out IStatData maxHealth);
            
            _healthBar.UpdateHealth(currentHealth.Value, maxHealth.Value);
        }
    }
}