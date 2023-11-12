using System.Text;

namespace Common.Dialogues.Utils
{
    public abstract class TextModifier
    {
        public abstract void BeginModification(string text, StringBuilder stringBuilder);
        public abstract void EndModification(string text, StringBuilder stringBuilder);
    }
}