using System;
using Core.PlayerInput;

namespace Core.Interfaces
{
    public interface IInputService : IService, IDisposable
    {
        Input Input { get; }
    }
}