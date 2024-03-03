using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitSharedData
    {
        int ID { get; }  
        
        Transform Transform { get; }
    }
}