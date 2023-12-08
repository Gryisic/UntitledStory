using System;
using UnityEngine;

namespace Common.Models.Animator.Callbacks
{
    public class DebugMessageCallback : AnimationCallback
    {
        [SerializeField] private string _message;
        
        public override void Execute()
        {
            Debug.Log(_message);
        }
    }
}