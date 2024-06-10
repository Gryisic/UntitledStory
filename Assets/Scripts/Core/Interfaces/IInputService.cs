using System;
using Core.PlayerInput;
using Infrastructure.Utils;

namespace Core.Interfaces
{
    public interface IInputService : IService, IDisposable
    {
        Input Input { get; }
        
        Enums.InputDevice CurrentDevice { get; }

        event Action<Enums.InputDevice> DeviceChanged;

        string InputToKey(Enums.Input input);
    }
}