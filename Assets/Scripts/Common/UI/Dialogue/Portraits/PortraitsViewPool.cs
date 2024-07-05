using System.Collections.Generic;
using Infrastructure.Factories.UIFactory;
using Infrastructure.Factories.UIFactory.Interfaces;
using UnityEngine;

namespace Common.UI.Dialogue.Portraits
{
    public class PortraitsViewPool
    {
        private readonly IUIFactory _uiFactory = new UIFactory();
        private readonly Queue<PortraitView> _portraits = new();

        private Transform _root;
        
        public void Initialize(Transform root, PortraitView originalView)
        {
            _root = root;
            
            _portraits.Enqueue(originalView);
        }
        
        public PortraitView GetView()
        {
            if (_portraits.Count > 1)
                return _portraits.Dequeue();

            PortraitView portrait = _portraits.Dequeue();
            PortraitView newPortrait = _uiFactory.CreateCopy<PortraitView>(portrait, _root);
            
            ReturnView(portrait);

            return newPortrait;
        }

        public void ReturnView(PortraitView view) => _portraits.Enqueue(view);
    }
}