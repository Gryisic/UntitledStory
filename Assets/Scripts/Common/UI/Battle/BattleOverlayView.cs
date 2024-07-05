using System.Threading;
using Core.Data.Texts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleOverlayView : AnimatableUIElement
    {
        [SerializeField] private BattleBordersView _bordersView;
        [SerializeField] private BattleThoughtsView _thoughtsView;
        [SerializeField] private PartyHealthView _partyHealthView;
        [SerializeField] private DecisionCounter _decisionsCounter;

        public BattleThoughtsView ThoughtsView => _thoughtsView;
        public PartyHealthView PartyHealthView => _partyHealthView;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            UniTask bordersTask = _bordersView.ActivateAsync(token);
            UniTask thoughtsTask = _thoughtsView.ActivateAsync(token);
            UniTask healthBarsTask = _partyHealthView.ActivateAsync(token);
            UniTask counterTask = _decisionsCounter.ActivateAsync(token);
            
            await UniTask.WhenAll(bordersTask, thoughtsTask, healthBarsTask, counterTask);
        }
        
        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            _partyHealthView.Deactivate();
            
            UniTask bordersTask = _bordersView.DeactivateAsync(token);
            UniTask thoughtsTask = _thoughtsView.DeactivateAsync(token);
            UniTask healthBarsTask = _partyHealthView.DeactivateAsync(token);
            UniTask counterTask = _decisionsCounter.DeactivateAsync(token);
            
            await UniTask.WhenAll(bordersTask, thoughtsTask, healthBarsTask, counterTask);
            
            Deactivate();
        }

        public void UpdateLocalization(GeneralMenuLocalization localization) =>
            _decisionsCounter.SetLocalization(localization.Entries["Decision"]);
    }
}