using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/QTE/Tap", fileName = "TapEvent")]
    public class TapQTETemplate : QuickTimeEventTemplate
    {
        public override Enums.QTEType Type => Enums.QTEType.Tap;
    }
}