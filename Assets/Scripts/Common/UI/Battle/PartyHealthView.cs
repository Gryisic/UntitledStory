using System.Collections.Generic;
using System.Threading;
using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Battle
{
    public class PartyHealthView : AnimatableUIElement
    {
        [SerializeField] private PartyMemberHealthBarView[] _views;

        private readonly Dictionary<IBattleUnitSharedData, PartyMemberHealthBarView> _activeViews = new();

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            await base.ActivateAsync(token);
            
            foreach (var view in _activeViews.Values) 
                view.ActivateAsync(token).Forget();
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            foreach (var view in _activeViews.Values) 
                view.DeactivateAsync(token).Forget();
            
            ClearViews();
            
            await base.DeactivateAsync(token);
        }

        public void SetUnitsData(IReadOnlyList<IBattleUnitSharedData> datas)
        {
            if (_activeViews.Count > 0) 
                ClearViews();
            
            for (var i = 0; i < datas.Count; i++)
            {
                IBattleUnitSharedData data = datas[i];
                PartyMemberHealthBarView view = _views[i];

                data.AppliedDamaged += OnHealthChanged;
                data.Healed += OnHealthChanged;
                
                _activeViews.Add(data, view);
                
                OnHealthChanged(data, 0);
            }
        }

        private void ClearViews()
        {
            foreach (var pair in _activeViews)
            {
                pair.Key.AppliedDamaged -= OnHealthChanged;
                pair.Key.Healed -= OnHealthChanged;
            }

            _activeViews.Clear();
        }

        private void OnHealthChanged(IImpactable impactable, int amount)
        {
            IBattleUnitSharedData unit = impactable as IBattleUnitSharedData;
            PartyMemberHealthBarView view = _activeViews[unit];
            
            unit.StatsHandler.GetHealthData(out IStatData currentHealth, out IStatData maxHealth);
            
            view.UpdateHealth(currentHealth.Value, maxHealth.Value);
        }
    }
}