using Infrastructure.Utils;

namespace Common.Dialogues.Utils
{
    public struct SpeakerData
    {
        private const string DefaultEmotion = "Neutral";
        
        public string Speaker { get; }
        public string Emotion { get; }
        public Enums.PortraitSide Side { get; }
        public bool LookAtCenter { get; }
        public bool IsDeactivatable { get; }

        public SpeakerData(string speaker, string emotion = DefaultEmotion, Enums.PortraitSide side = Enums.PortraitSide.Free, bool lookAtCenter = true, bool isDeactivatable = false)
        {
            Speaker = speaker;
            Emotion = emotion;
            Side = side;
            LookAtCenter = lookAtCenter;
            IsDeactivatable = isDeactivatable;
        }
    }
}