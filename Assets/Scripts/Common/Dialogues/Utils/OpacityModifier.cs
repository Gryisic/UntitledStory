using System.Text;
using Core.Extensions;

namespace Common.Dialogues.Utils
{
    public class OpacityModifier : TextModifier
    {
        public override void BeginModification(string text, StringBuilder stringBuilder)
        {
            int.TryParse(text, out int value);
            
            stringBuilder.StartOpacity(value);
        }

        public override void EndModification(string text, StringBuilder stringBuilder) => stringBuilder.EndOpacity();
    }
}