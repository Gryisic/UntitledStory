using Common.UI.Common;
using DG.Tweening;
using UnityEngine;

namespace Common.UI.Enemy
{
    public class EnemyHealthBarView : HealthBarView
    {
        [SerializeField] private SpriteRenderer _bar;
        
        private readonly int _fillAmount = Shader.PropertyToID("_FillAmount");
        
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            
            FillBar(1);
            
            Deactivate();
        }

        public override void Activate()
        {
            base.Activate();

            transform.DOPunchRotation(new Vector3(0f, 0f, 2.5f), 0.6f);
        }

        public override void UpdateHealth(int currentHealth, int maxHealth)
        {
            float fillAmount = (float) currentHealth / maxHealth;
            
            FillBar(fillAmount);
        }

        private void FillBar(float amount)
        {
            _propertyBlock.SetFloat(_fillAmount, amount);
            
            _bar.SetPropertyBlock(_propertyBlock);
        }
    }
}