using System;
using System.Text;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.Data.Icons
{
    public class InputIcons : Icons
    {
        [SerializeField] private InputIconsMap<InputEnums.Keyboard> _keyboardIcons;
        [SerializeField] private InputIconsMap<InputEnums.MicrosoftGamepad> _microsoftIcons;
        [SerializeField] private InputIconsMap<InputEnums.SonyGamepad> _sonyIcons;

        private readonly StringBuilder _iconNameBuilder = new ();
        
        private IInputService _inputService;

        public IconsMap DeviceIcons { get; private set; }

#if UNITY_EDITOR

        public static string KeyboardIconsPropertyName => nameof(_keyboardIcons);
        public static string MicrosoftIconsPropertyName => nameof(_microsoftIcons);
        public static string SonyIconsPropertyName => nameof(_sonyIcons);
        
#endif

        public void Initialize(IInputService inputService)
        {
            DeviceIcons = _keyboardIcons;
            _inputService = inputService;
        }
        
        public Sprite GetPressedIcon(Enums.Input input) => GetIcon(input, Constants.PressedInputButtonDeclaration);

        public Sprite GetReleasedIcon(Enums.Input input) => GetIcon(input, Constants.ReleasedInputButtonDeclaration);

        public void UpdateActiveMap(Enums.InputDevice device)
        {
            switch (device)
            {
                case Enums.InputDevice.KeyboardAndMouse:
                    DeviceIcons = _keyboardIcons;
                    break;

                case Enums.InputDevice.SonyGamepad:
                    DeviceIcons = _sonyIcons;
                    break;

                case Enums.InputDevice.MicrosoftGamepad:
                    DeviceIcons = _microsoftIcons;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(device), device, null);
            }
        }

        private Sprite GetIcon(Enums.Input input, string declaration)
        {
            _iconNameBuilder.Clear()
                .Append(_inputService.InputToKey(input))
                .Append(declaration);

            string iconName = _iconNameBuilder.ToString();
            
            return DeviceIcons.GetIcon(iconName);
        }
    }
}