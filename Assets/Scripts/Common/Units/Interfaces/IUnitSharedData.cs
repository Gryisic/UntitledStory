using System;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitSharedData
    {
        event Action<IUnitSharedData> Dead; 

        int ID { get; }  
        
        Transform Transform { get; }
        
        bool IsDead { get; }
    }
}