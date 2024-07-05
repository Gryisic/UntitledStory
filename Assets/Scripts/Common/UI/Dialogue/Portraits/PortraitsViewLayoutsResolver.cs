using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Dialogue.Portraits
{
    public class PortraitsViewLayoutsResolver
    {
        private readonly Vector3 _leftSideCenter = new(-620, -220, 0);
        private readonly Vector3 _rightSideCenter = new(620, -220, 0);
        
        private readonly PortraitsViewLayout _leftSideLayout;
        private readonly PortraitsViewLayout _rightSideLayout;

        private Enums.PortraitSide _lastSide = Enums.PortraitSide.Right;

        public PortraitsViewLayoutsResolver()
        {
            _leftSideLayout = new PortraitsViewLayout(Enums.PortraitSide.Left);
            _rightSideLayout = new PortraitsViewLayout(Enums.PortraitSide.Right);
            
            _leftSideLayout.SetCentralPosition(_leftSideCenter);
            _rightSideLayout.SetCentralPosition(_rightSideCenter);
        }

        public void UpdatePosition(PortraitView view, Enums.PortraitSide side, bool lookAtCenter)
        {
            if (side == Enums.PortraitSide.Free)
            {
                if (SideHasSpeaker(view, lookAtCenter))
                    return;

                Enums.PortraitSide newSide = _lastSide == Enums.PortraitSide.Left
                    ? Enums.PortraitSide.Right
                    : Enums.PortraitSide.Left;

                SetOnSide(view, newSide, lookAtCenter);
                
                return;
            }
            
            SetOnSide(view, side, lookAtCenter);
        }

        public void Remove(PortraitView view)
        {
            if (_leftSideLayout.Remove(view))
                return;

            _rightSideLayout.Remove(view);
        }

        public void Clear()
        {
            _leftSideLayout.Clear();
            _rightSideLayout.Clear();

            _lastSide = Enums.PortraitSide.Right;
        }

        private bool SideHasSpeaker(PortraitView view, bool lookAtCenter)
        {
            if (_leftSideLayout.HasSpeakerOnSide(view.Data.Speaker, out Enums.PortraitSide speakerSide) ||
                _rightSideLayout.HasSpeakerOnSide(view.Data.Speaker, out speakerSide))
            {
                SetOnSide(view, speakerSide, lookAtCenter);

                return true;
            }

            return false;
        }
        
        private void SetOnSide(PortraitView view, Enums.PortraitSide side, bool lookAtCenter)
        {
            switch (side)
            {
                case Enums.PortraitSide.Left:
                    ToLayout(view, lookAtCenter, _leftSideLayout, _rightSideLayout);
                    
                    _lastSide = Enums.PortraitSide.Left;
                    break;
                
                case Enums.PortraitSide.Right:
                    ToLayout(view, lookAtCenter, _rightSideLayout, _leftSideLayout);

                    _lastSide = Enums.PortraitSide.Right;
                    break;
            }
        }

        private void ToLayout(PortraitView view, bool lookAtCenter, PortraitsViewLayout target, PortraitsViewLayout opposite)
        {
            if (opposite.HasSpeakerOnSide(view.Data.Speaker, out _))
                Switch(view, lookAtCenter, opposite, target);
            else
                target.Add(view, lookAtCenter);

            opposite.SetPassive();
        }

        private void Switch(PortraitView view, bool lookAtCenter, PortraitsViewLayout from, PortraitsViewLayout to)
        {
            from.Remove(view);
            to.AddSmooth(view, lookAtCenter);
        }
    }
}