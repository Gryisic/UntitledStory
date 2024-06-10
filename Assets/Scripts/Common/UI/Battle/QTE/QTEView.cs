using System;
using System.Collections.Generic;
using Common.UI.Interfaces;
using Core.Data.Icons;
using Core.Data.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle.QTE
{
    public class QTEView : UIElement, IIconsRequester, IDisposable
    {
        [SerializeField] private QTEViewPool _viewPool;

        private readonly Dictionary<int, ConcreteQTEView> _viewMap = new();

        private IIconsData _iconsData;

        public void Dispose()
        {
            _viewPool?.Dispose();
        }
        
        public void SetIconsData(IIconsData iconsData)
        {
            _iconsData = iconsData;
        }
        
        public ConcreteQTEView GetViewOfTypeAndSaveToMap(Enums.QTEType type, int hashcode)
        {
            ConcreteQTEView view = _viewPool.Get<ConcreteQTEView>(type);

            view.SetIcons(_iconsData.GetIcons<InputIcons>());
            _viewMap.Add(hashcode, view);

            return view;
        }

        public bool TryGetViewByHash(int hashcode, out ConcreteQTEView view) 
            => _viewMap.TryGetValue(hashcode, out view);

        public void ReturnViewOfType(Enums.QTEType type, int hashcode)
        {
            TryGetViewByHash(hashcode, out ConcreteQTEView view);
            
            _viewPool.Return(type, view);
            _viewMap.Remove(hashcode);
        }
    }
}