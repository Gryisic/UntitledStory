using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.PlayerInput
{
    public class InputService : IInputService
    {
        public Input Input { get; }
        public Enums.InputDevice CurrentDevice { get; private set; }

        private readonly InputMap _map;
        
        public event Action<Enums.InputDevice> DeviceChanged;

        public InputService(Input input)
        {
            Input = input;

            _map = new InputMap();
            
            Initialize();
        }

        private void Initialize()
        {
            InputSystem.onActionChange += OnActionPerformed;
        }
        
        public void Dispose()
        {
            InputSystem.onActionChange -= OnActionPerformed;
            
            Input?.Dispose();
        }

        public string InputToKey(Enums.Input input) => _map.GetActionName(input);

        private void OnActionPerformed(object data, InputActionChange change)
        {
            if (change != InputActionChange.ActionStarted) 
                return;
            
            InputAction lastAction = (InputAction) data;
            InputControl lastControl = lastAction.activeControl;
            InputDevice lastDevice = lastControl.device;

            Enums.InputDevice device = DefineInputDevice(lastDevice.name);
            
            if (device == CurrentDevice) 
                return;
            
            CurrentDevice = device;
                
            DeviceChanged?.Invoke(device);
        }

        private Enums.InputDevice DefineInputDevice(string deviceName)
        {
            return deviceName switch
            {
                "Keyboard" => Enums.InputDevice.KeyboardAndMouse,
                "Mouse" => Enums.InputDevice.KeyboardAndMouse,
                "PlaystationController" => Enums.InputDevice.SonyGamepad,
                _ => Enums.InputDevice.MicrosoftGamepad
            };
        }
    }
}