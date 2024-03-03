using System;
using System.Collections.Generic;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle.QTE
{
    public class QTEView : UIElement, IDisposable
    {
        [SerializeField] private QTEViewPool _viewPool;

        private readonly Dictionary<int, ConcreteQTEView> _viewMap = new();

        public void Dispose()
        {
            _viewPool?.Dispose();
        }
        
        public ConcreteQTEView GetViewOfTypeAndSaveToMap(Enums.QTEType type, int hashcode)
        {
            ConcreteQTEView view = _viewPool.Get<ConcreteQTEView>(type);

            _viewMap.Add(hashcode, view);

            return view;
        }

        public ConcreteQTEView GetViewByHash(int hashcode)
        {
            if (_viewMap.TryGetValue(hashcode, out ConcreteQTEView view))
                return view;
            
            throw new NullReferenceException($"QTE View map doesn't contain view with hash: {hashcode}");
        }
        
        public void ReturnViewOfType(Enums.QTEType type, int hashcode)
        {
            ConcreteQTEView view = GetViewByHash(hashcode);
            
            _viewPool.Return(type, view);
            _viewMap.Remove(hashcode);
        }
    }
}