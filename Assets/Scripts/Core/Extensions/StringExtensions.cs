using System.Linq;
using System.Text;
using UnityEngine;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstUpper(this string text, bool clearSpecialSymbols = false, bool addSpaces = true)
        {
            if (clearSpecialSymbols)
                text = new string(text
                    .Where(letter => char.IsWhiteSpace(letter) || char.IsLetterOrDigit(letter))
                    .ToArray());

            char[] letters = text.ToCharArray();
            char upperLetter = char.ToUpper(letters[0]);
            
            letters[0] = upperLetter;

            text = addSpaces ? new string(letters).WithSpaces() : new string(letters);
            
            return text;
        }

        public static string WithSpaces(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            StringBuilder spacedTextBuilder = new StringBuilder(text.Length * 2);
            spacedTextBuilder.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    spacedTextBuilder.Append(' ');
                spacedTextBuilder.Append(text[i]);
            }
            
            return spacedTextBuilder.ToString();
        }
    }
}