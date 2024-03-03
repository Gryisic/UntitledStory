using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.Templates
{
    public abstract class QuickTimeEventTemplate : ScriptableObject
    {
        [SerializeField] private Sprite _pressed;
        [SerializeField] private Sprite _released;

        [SerializeField] private float _startDelay;
        [SerializeField] private float _duration;
        [SerializeField] private float _openDelay;

        [SerializeField] private Enums.TargetSide _targetSide;
        [SerializeField] private Enums.QTEOffset _offset;
        [SerializeField] private Vector2 _offsetValue;

        [SerializeField] private Enums.QTEInput _input;

        public abstract Enums.QTEType Type { get; }
        
        public Sprite Pressed => _pressed;
        public Sprite Released => _released;
        
        public float StartDelay => _startDelay;
        public float Duration => _duration;
        public float OpenDelay => _openDelay;
        
        public Enums.TargetSide TargetSide => _targetSide;
        public Enums.QTEOffset Offset => _offset;
        public Vector2 OffsetValue => _offsetValue;

        public Enums.QTEInput Input => _input;
    }
}