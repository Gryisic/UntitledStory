using System;

namespace Common.Models.Triggers.Interfaces
{
    public interface ITrigger
    {
        int ID { get; }
        
        event Action Triggered;
    }
}