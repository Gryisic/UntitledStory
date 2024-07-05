using System;
using System.Collections.Generic;
using System.Text;
using Core.Extensions;
using Infrastructure.Utils;

namespace Common.Dialogues.Utils
{
    public class InkStoryParser
    {
        private const string SpeakerTag = "s";
        private const string DeactivationKey = "deactivate";

        private const char ModifierEndChar = '/';
        private const char TagsDelimiterChar = ':';
        private readonly char[] _delimiterChars = { '[', ']' };
        private readonly char[] _innerDelimiterChars = { '_' };

        private readonly Dictionary<string, TextModifier> _textModifiersMap = new()
        {
            {"color", new ColorModifier()},
            {"opacity", new OpacityModifier()}
        };
        
        private readonly Dictionary<string, Enums.PortraitSide> _speakerSideMap = new()
        {
            {"free", Enums.PortraitSide.Free},
            {"left", Enums.PortraitSide.Left},
            {"right", Enums.PortraitSide.Right}
        };
        
        private readonly Dictionary<string, bool> _speakerCenterMap = new()
        {
            {"true", true},
            {"false", false}
        };

        private readonly StringBuilder _stringBuilder;

        private string _rawText;
        
        public InkStoryParser()
        {
            _stringBuilder = new StringBuilder();
        }
        
        public void Parse(string initialText, IReadOnlyList<string> tags, out InkParsedText parsedText)
        {
            parsedText = new InkParsedText();
            
            ParseTags(parsedText, tags);
            
            string[] sentences = initialText.Split(_delimiterChars);

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
                    
                    _textModifiersMap[firstSubSentence.Remove(0, 1)].EndModification(firstSubSentence.ToLower(), _stringBuilder);
                    
                    int endIndex = _stringBuilder.Length - 1;
                    
                    parsedText.AddHiddenPart(startIndex, endIndex);
                    
                    continue;
                }
                
                if (_textModifiersMap.TryGetValue(firstSubSentence.ToLower(), out TextModifier modifier))
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

        private void ParseTags(InkParsedText text, IReadOnlyList<string> tags)
        {
            foreach (var tag in tags)
            {
                string[] splittedTag = tag.Split(TagsDelimiterChar);
                string concreteTag = splittedTag[0].Trim();
                string tagValue = splittedTag[1].Trim();
                
                if (splittedTag.Length < 2)
                    throw new Exception($"Invalid 'Ink' tag. Tag: {tag}");
                
                switch (concreteTag)
                {
                    case SpeakerTag:
                        DefineSpeaker(text, tagValue);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException($"Founded an unknown tag: {concreteTag}");
                }
            }
        }

        private void DefineSpeaker(InkParsedText text, string tag)
        {
            string[] values = tag.Split(_innerDelimiterChars);
            string speaker = values[0];
            
            SpeakerData data = new SpeakerData(speaker);
            
            for (var i = 1; i < values.Length; i++)
            {
                string value = values[i].ToLower();

                data = UpdateSpeakerData(value, data);
            }
            
            text.SetSpeaker(data);
        }

        private SpeakerData UpdateSpeakerData(string value, SpeakerData data)
        {
            if (_speakerSideMap.TryGetValue(value, out Enums.PortraitSide side))
            {
                data = new SpeakerData(data.Speaker, data.Emotion, side, data.LookAtCenter);

                return data;
            }
                
            if (_speakerCenterMap.TryGetValue(value, out bool lookAtCenter))
            {
                data = new SpeakerData(data.Speaker, data.Emotion, data.Side, lookAtCenter);

                return data;
            }

            if (value == DeactivationKey)
            {
                data = new SpeakerData(data.Speaker, data.Emotion, data.Side, data.LookAtCenter, true);

                return data;
            }
                
            data = new SpeakerData(data.Speaker, value.ToFirstUpper(), data.Side, data.LookAtCenter);
            
            return data;
        }
    }
}