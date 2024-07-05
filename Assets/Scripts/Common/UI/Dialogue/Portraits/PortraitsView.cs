using System.Collections.Generic;
using System.Threading;
using Common.Dialogues.Utils;
using Core.Data.Icons;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Dialogue.Portraits
{
    public class PortraitsView : AnimatableUIElement
    {
        [SerializeField] private PortraitView _originalPortrait;
        
        private PortraitsViewPool _viewPool;
        private List<PortraitView> _activePortraits;
        private PortraitsViewLayoutsResolver _layoutsResolver;
        
        private PortraitIcons _icons;

        private CancellationToken _token;

        private void Awake()
        {
            _viewPool = new PortraitsViewPool();
            _activePortraits = new List<PortraitView>();
            _layoutsResolver = new PortraitsViewLayoutsResolver();
            
            _viewPool.Initialize(transform, _originalPortrait);
            
            _originalPortrait.Deactivate();
        }

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _token = token;
            
            await base.ActivateAsync(token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            List<UniTask> portraitsTasks = new List<UniTask>();

            _activePortraits.ForEach(p => portraitsTasks.Add(p.DeactivateAsync(token)));
            
            await UniTask.WhenAll(portraitsTasks);
            
            _activePortraits.ForEach(p => _viewPool.ReturnView(p));
            
            _activePortraits.Clear();
            _layoutsResolver.Clear();
            
            await base.DeactivateAsync(token);
        }

        public void SetIcons(PortraitIcons icons) => _icons = icons;

        public void UpdatePortrait(SpeakerData args)
        {
            PortraitView portrait = _activePortraits.Find(p => p.Data.Speaker == args.Speaker);
            Sprite portraitIcon = _icons.GetPortrait(args.Speaker, args.Emotion);

            if (portraitIcon == null)
                return;
            
            if (portrait == null)
            {
                portrait = _viewPool.GetView();
                
                _activePortraits.Add(portrait);

                portrait.ActivateAsync(_token).Forget();
            }
            
            _layoutsResolver.UpdatePosition(portrait, args.Side, args.LookAtCenter);
            
            portrait.UpdateData(args);
            portrait.UpdatePortrait(portraitIcon);
        }

        public void DeactivatePortrait(SpeakerData args)
        {
            if (args.IsDeactivatable == false)
                return;
            
            PortraitView portrait = _activePortraits.Find(p => p.Data.Speaker == args.Speaker);

            _layoutsResolver.Remove(portrait);
            portrait.DeactivateAsync(_token).Forget();

            _activePortraits.Remove(portrait);
            _viewPool.ReturnView(portrait);
        }
    }
}