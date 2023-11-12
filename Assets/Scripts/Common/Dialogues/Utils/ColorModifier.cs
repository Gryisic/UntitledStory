using System.Collections.Generic;
using System.Text;
using Core.Extensions;

namespace Common.Dialogues.Utils
{
    public class ColorModifier : TextModifier
    {
        private readonly Dictionary<string, string> _colorMap = new Dictionary<string, string>()
        {
            {"red", "F11414"},
            {"orange", "FF7D00"}
        };
        
        public override void BeginModification(string text, StringBuilder stringBuilder) => stringBuilder.StartColor(_colorMap[text]);

        public override void EndModification(string text, StringBuilder stringBuilder) => stringBuilder.EndColor();
    }
}