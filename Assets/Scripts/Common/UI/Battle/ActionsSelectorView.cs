﻿using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Interfaces;
using Common.Battle.TargetSelection;
using Common.Battle.TargetSelection.Interfaces;
using Common.UI.Interfaces;
using Core.Data.Texts;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;

namespace Common.UI.Battle
{
    public class ActionsSelectorView : AnimatableUIElement, ITargetSelectionRequester, IQueueableUIElement, ISelectableUIElement
    {
        [SerializeField] private TextMeshProUGUI _phrase;
        [SerializeField] private List<ActionButtonView> _buttons;

        private const float ActivationToggleDelay = Constants.DefaultUITweenTime / 3;
        
        public event Action<UIElement> RequestAddingToQueue;
        public event Action<ITargetSelectionData> RequestTargetSelection;
        public event Action SuppressTargetSelection;

        public override void Activate()
        {
            base.Activate();
            
            RequestTargetSelection?.Invoke(new DefaultTargetSelection());
            RequestAddingToQueue?.Invoke(this);
        }

        public override void Deactivate()
        {
            SuppressTargetSelection?.Invoke();

            base.Deactivate();
        }

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();

            List<UniTask> activateTasks = new List<UniTask>();
            UniTask task = _phrase
                .DOFade(1, Constants.DefaultUITweenTime).From(0)
                .ToUniTask(cancellationToken: token);
            
            _phrase.gameObject.SetActive(true);
            
            activateTasks.Add(task);

            foreach (var button in _buttons)
            {
                task = button.ActivateAsync(token);
                
                activateTasks.Add(task);

                await UniTask.Delay(TimeSpan.FromSeconds(ActivationToggleDelay), cancellationToken: token);
            }

            await UniTask.WhenAll(activateTasks);
        }
        
        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            List<UniTask> deactivateTasks = new List<UniTask>();

            for (var i = _buttons.Count - 1; i >= 0; i--)
            {
                ActionButtonView button = _buttons[i];
                UniTask task = button.DeactivateAsync(token);

                deactivateTasks.Add(task);

                await UniTask.Delay(TimeSpan.FromSeconds(ActivationToggleDelay), cancellationToken: token);
            }

            UniTask nameTask = _phrase
                .DOFade(0, Constants.DefaultUITweenTime).From(1)
                .ToUniTask(cancellationToken: token);
            
            deactivateTasks.Add(nameTask);
            
            await UniTask.WhenAll(deactivateTasks);
            
            _phrase.gameObject.SetActive(false);
            Deactivate();
        }

        public void UpdateActionPhrase(PartyMemberLocalization partyLocalization) => 
            _phrase.text = partyLocalization.ActionPhrase;

        public void UpdateButtonsLocalization(GeneralMenuLocalization menusLocalization) => 
            _buttons.ForEach(b => b.UpdateLocalization(menusLocalization));

        public void UpdateRotation(Enums.BattleFieldSide side)
        {
            _phrase.alignment = side == Enums.BattleFieldSide.Left
                ? TextAlignmentOptions.Right
                : TextAlignmentOptions.Left;
            
            _buttons.ForEach(b => b.Flip(side));
        }
        
        public void Select() => SuppressTargetSelection?.Invoke();

        public void Back() { }
    }
}