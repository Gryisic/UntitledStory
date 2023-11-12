using System.Collections.Generic;
using UnityEngine;

namespace Common.Dialogues.Utils
{
    public class InkParsedText
    {
        private readonly List<HiddenPart> _hiddenParts;
        private int _hiddenPartIndex;
        
        public string Sentence { get; private set; }

        public int NextHiddenPartIndex => _hiddenParts[_hiddenPartIndex].StartIndex;
        public int NextVisiblePartIndex => _hiddenParts[_hiddenPartIndex].EndIndex;

        public bool HasHiddenParts => _hiddenParts.Count > 0;

        public InkParsedText()
        {
            _hiddenParts = new List<HiddenPart>();
        }

        public void AddHiddenPart(int startIndex, int endIndex) => _hiddenParts.Add(new HiddenPart(startIndex, endIndex));

        public void ToNextHiddenPart() => _hiddenPartIndex = _hiddenPartIndex + 1 >= _hiddenParts.Count ? _hiddenParts.Count - 1 : _hiddenPartIndex + 1;

        public void SetSentence(string sentence) => Sentence = sentence;

        private struct HiddenPart
        {
            public int StartIndex { get; }
            public int EndIndex { get; }
            
            public HiddenPart(int startIndex, int endIndex)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
        }
    }
}