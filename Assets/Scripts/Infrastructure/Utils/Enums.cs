using System;

namespace Infrastructure.Utils
{
    public static class Enums
    {
        public enum GameStateType
        {
            Initialize,
            SceneSwitch,
            Explore,
            Dialogue
        }
        
        public enum Language
        {
            Russian,
            English
        }

        public enum SceneType
        {
            Academy
        }

        public enum CameraDistanceType
        {
            Neutral,
            Far,
            Close
        }

        public enum CameraEasingType
        {
            Instant,
            Smooth
        }

        public enum UILayer
        {
            Overlay,
            Camera,
            World
        }

        public enum TriggerActivationType
        {
            Auto,
            Manual
        }
        
        public enum TriggerLoopType
        {
            OneShot,
            Cycle
        }
    }
}