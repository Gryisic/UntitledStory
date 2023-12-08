using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.Templates
{
    [CreateAssetMenu(menuName = "Common/Templates/QTE/Hold", fileName = "HoldEvent")]
    public class HoldQTETemplate : QuickTimeEventTemplate
    {
        [SerializeField] private float _holdDuration;

        public override Enums.QTEType Type => Enums.QTEType.Hold;

        public float HoldDuration => _holdDuration;
    }
}