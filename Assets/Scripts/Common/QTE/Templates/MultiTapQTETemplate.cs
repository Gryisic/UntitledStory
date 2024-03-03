using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.Templates
{
    public class MultiTapQTETemplate : QuickTimeEventTemplate
    {
        [SerializeField] private int _tapCount;
        
        public override Enums.QTEType Type => Enums.QTEType.MultiTap;
        public int TapCount => _tapCount;
    }
}