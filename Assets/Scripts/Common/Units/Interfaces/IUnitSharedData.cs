using System;
using Common.Models.Impactable.Interfaces;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitSharedData 
    {
        int ID { get; }  
        
        Transform Transform { get; }
    }
}