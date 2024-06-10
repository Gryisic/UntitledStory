using Common.QTE.EndBehaviour;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.QTE.Templates
{
    public abstract class QuickTimeEventTemplate : ScriptableObject
    {
        [SerializeField, ExposedRange(0, Constants.MaxQTEStartDelay)] private float _startDelay;
        [SerializeField, ExposedRange(0.1f, Constants.MaxQTEDuration)] private float _duration;
        [SerializeField] private float _openDelay;

        [SerializeField] private Enums.TargetSide _targetSide;
        [SerializeField] private Enums.QTEOffset _offset;
        [SerializeField] private Vector2 _offsetValue;

        [SerializeField] private Enums.Input _input;

        [SerializeReference, SubclassesPicker] private QTEEndBehaviour _endBehaviour;

        private float _startDelayMin;

        public abstract Enums.QTEType Type { get; }

        public float StartDelay => _startDelay;
        public float Duration => _duration;
        public float OpenDelay => _openDelay;
        
        public Enums.TargetSide TargetSide => _targetSide;
        public Enums.QTEOffset Offset => _offset;
        public Vector2 OffsetValue => _offsetValue;

        public Enums.Input Input => _input;
        
        public QTEEndBehaviour EndBehaviour => _endBehaviour;
        
#if UNITY_EDITOR
        public static string OffsetPropertyName => nameof(_offset);
        public static string OffsetValuePropertyName => nameof(_offsetValue);
        public static string TargetSidePropertyName => nameof(_targetSide);
        public static string DurationPropertyName => nameof(_duration);
        public static string StartDelayPropertyName => nameof(_startDelay);

        public void SetMinDelay(float delay)
        {
            _startDelayMin = delay;

            if (_startDelay < _startDelayMin)
                _startDelay = _startDelayMin;
        }
#endif
    }
}