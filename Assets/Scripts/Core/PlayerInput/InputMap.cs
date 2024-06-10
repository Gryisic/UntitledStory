using System.Collections.Generic;
using Infrastructure.Utils;

namespace Core.PlayerInput
{
    public class InputMap
    {
        private readonly Dictionary<Enums.Input, string> _map = new()
        {
            { Enums.Input.A , "Q" }, { Enums.Input.B , "E" }, { Enums.Input.Y , "X" }, { Enums.Input.X , "Z" },
            { Enums.Input.Up , "W" }, { Enums.Input.Down , "S" }, { Enums.Input.Left , "A" }, { Enums.Input.Right , "D" },
            { Enums.Input.LB , "R" }, { Enums.Input.LT , "T" },
            { Enums.Input.RB , "F" }, { Enums.Input.RT , "G" },
            { Enums.Input.Start , "Enter" }, { Enums.Input.Select , "Backspace" },
        };
        
        public string GetActionName(Enums.Input input) => _map[input];
    }
}