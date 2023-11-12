using System.Collections.Generic;
using System.Text;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Dialogues.Utils
{
    public class InkStoryParser
    {
        private const char ModifierEndChar = '/';
        private readonly char[] _delimiterChars = { '[', ']' };
        private readonly char[] _innerDelimiterChars = { '_' };

        private readonly Dictionary<string, TextModifier> _textModifiersMap = new Dictionary<string, TextModifier>()
        {
            {"color", new ColorModifier()},
            {"opacity", new OpacityModifier()}
        };

        private readonly StringBuilder _stringBuilder;

        private string _rawText;
        
        public InkStoryParser()
        {
            _stringBuilder = new StringBuilder();
        }
        
        public void Parse(string initialText, out InkParsedText parsedText)
        {
            string[] sentences = initialText.Split(_delimiterChars);
            parsedText = new InkParsedText();

            if (sentences.Length == 1)
            {
                parsedText.SetSentence(initialText);

                return;
            }
            
            _stringBuilder.Clear();

            foreach (var sentence in sentences)
            {
                string[] subSentences = sentence.Split(_innerDelimiterChars);
                string firstSubSentence = subSentences[0];
                
                if (firstSubSentence == string.Empty)
                    continue;
                
                if (firstSubSentence[0] == ModifierEndChar)
                {
                    int startIndex = _stringBuilder.Length;
                    
                    _textModifiersMap[firstSubSentence.Remove(0, 1)].EndModification(firstSubSentence, _stringBuilder);
                    
                    int endIndex = _stringBuilder.Length - 1;
                    
                    parsedText.AddHiddenPart(startIndex, endIndex);
                    
                    continue;
                }
                
                if (_textModifiersMap.TryGetValue(firstSubSentence, out TextModifier modifier))
                {
                    int startIndex = _stringBuilder.Length;

                    modifier.BeginModification(subSentences[1], _stringBuilder);

                    int endIndex = _stringBuilder.Length - 1;
                    
                    parsedText.AddHiddenPart(startIndex, endIndex);
                    
                    continue;
                }
                
                _stringBuilder.Append(sentence);
            }
            
            parsedText.SetSentence(_stringBuilder.ToString());
        }
    }
}