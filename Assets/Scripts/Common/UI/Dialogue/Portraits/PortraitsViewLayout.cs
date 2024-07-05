using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Dialogue.Portraits
{
    public class PortraitsViewLayout
    {
        private const float ScaleOffset = 0.8f;
        
        private readonly Color _activeColor = Color.white;
        private readonly Color _passiveColor = new(0.6f, 0.6f, 0.6f);
        
        private List<PortraitView> _views;

        private Vector3 _centralPosition;
        private Vector3 _centerRotation;
        private Vector3 _outOfCenterRotation;
        
        private Vector3 _positionOffset = new(-100, -80, 0);
        
        public Enums.PortraitSide Side { get; }
        
        public PortraitsViewLayout(Enums.PortraitSide side)
        {
            _views = new List<PortraitView>();

            Side = side;
        }

        public void SetCentralPosition(Vector3 centralPosition)
        {
            _centralPosition = centralPosition;
            _centerRotation = _centralPosition.x > 0 ? Vector3.zero : new Vector3(0, 180, 0);
            _outOfCenterRotation = _centralPosition.x > 0 ? new Vector3(0, 180, 0) : Vector3.zero;
            _positionOffset = _centralPosition.x > 0
                ? new Vector3(_positionOffset.x * -1, _positionOffset.y, _positionOffset.z)
                : _positionOffset;
        }

        public bool HasSpeakerOnSide(string speaker, out Enums.PortraitSide side)
        {
            side = Side;
            
            return _views.Find(v => v.Data.Speaker == speaker);
        }

        public void AddSmooth(PortraitView view, bool lookAtCenter)
        {
            Add(view, lookAtCenter);
        }
        
        public void Add(PortraitView view, bool lookAtCenter)
        {
            view.Transform.SetAsLastSibling();
            
            if (_views.Contains(view) == false) 
                _views.Add(view);
            
            Sort();

            view.SetColor(_activeColor);

            Vector3 finalRotation = lookAtCenter ? _centerRotation : _outOfCenterRotation;
            
            view.Transform.rotation = Quaternion.Euler(finalRotation);
        }

        public bool Remove(PortraitView view)
        {
            bool removed = _views.Remove(view);
            
            if (removed)
                Sort();
            
            return removed;
        }

        public void Clear() => _views.Clear();

        public void SetPassive()
        {
            if (_views.Count <= 0)
                return;
            
            _views[0].SetColor(_passiveColor);
        }

        private void Sort()
        {
            _views = _views.OrderByDescending(v => v.Transform.GetSiblingIndex()).ToList();

            for (var i = 0; i < _views.Count; i++)
            {
                PortraitView portraitView = _views[i];
                
                Move(portraitView, i);
            }
        }

        private void Move(PortraitView view, int index)
        {
            view.SetColor(_passiveColor);
            view.Transform.localPosition = _centralPosition + _positionOffset * index;
            view.Transform.localScale = Vector3.one * Mathf.Pow(ScaleOffset, index);
        }
    }
}