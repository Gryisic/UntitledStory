﻿using System;

namespace Infrastructure.Utils
{
    public static class Enums
    {
        public enum GameStateType
        {
            Initialize,
            SceneSwitch,
            Explore,
            Dialogue,
            Battle
        }
        
        public enum Language
        {
            Russian,
            English
        }
        
        public enum InkFunction
        {
            DebugMessage,
            ActivateTrigger,
            DeactivateTrigger
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

        public enum ListedItem
        {
            Item,
            BattleAction
        }

        public enum QTEState
        {
            Sleep,
            Started,
            Opened,
            Succeeded,
            Failed
        }
        
        public enum QTEType
        {
            Tap,
            Hold
        }
        
        public enum QTEInput
        {
            Attack,
            Skill
        }
        
        public enum BattleActions
        {
            Attack,
            Skill,
            Guard,
            Items
        }

        public enum BattleActionEffect
        {
            Damage,
            Heal,
            Buff,
            Debuff,
            Resurrection,
            EffectsClear
        }
        
        public enum Buff
        {
            Regeneration,
            Concentration
        }
        
        public enum Debuff
        {
            Bleeding,
            Corruption
        }
        
        public enum TargetSide
        {
            SameAsUnit,
            OppositeToUnit
        }

        public enum TargetsQuantity
        {
            Single,
            All
        }

        public enum BattleConstraint
        {
            Placement,
            ExternalUnits
        }

        public enum BattleFieldSide
        {
            Left, 
            Right
        }

        public enum BattleFieldPlacement
        {
            Random,
            Fixed
        }
        
        public enum BattleTurn
        {
            Party,
            Enemy
        }

        public enum TriggerPriority
        {
            Main,
            Sub
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

        public enum StandardAnimation
        {
            Idle,
            Run,
            TakeDamage
        }
    }
}