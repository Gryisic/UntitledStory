using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.Templates
{
    public abstract class QuickTimeEventTemplate : ScriptableObject
    {
        [SerializeField] private float _startDelay;
        [SerializeField] private float _duration;
        [SerializeField] private float _openDelay;

        [SerializeField] private Enums.QTEInput _input;

        public abstract Enums.QTEType Type { get; }
        
        public float StartDelay => _startDelay;
        public float Duration => _duration;
        public float OpenDelay => _openDelay;

        public Enums.QTEInput Input => _input;
    }
}