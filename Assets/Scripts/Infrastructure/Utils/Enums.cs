using System;
using Unity.Behavior;

namespace Infrastructure.Utils
{
    public static class Enums
    {
        [BlackboardEnum]
        public enum GameStateType
        {
            Initialize,
            Finalize,
            SceneSwitch,
            Explore,
            Dialogue,
            Battle
        }
        
        public enum GameStateFinalization
        {
            Full,
            Partial
        }

        public enum PostEventState
        {
            Default,
            Previous,
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
        
        public enum LocalizableText
        {
            BattleThought
        }

        public enum InputDevice
        {
            KeyboardAndMouse,
            SonyGamepad,
            MicrosoftGamepad
        }
        
        public enum ValueComparisonMethod
        {
            Integer,
            Percent
        }
        
        public enum ValueComparator
        {
            Equals,
            Greater,
            GreaterOrEquals,
            Less,
            LessOrEquals
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
        
        public enum CameraCenterPositioning
        {
            Default,
            Center
        }

        public enum UILayer
        {
            Overlay,
            Camera,
            World,
            Constructed
        }

        public enum ListedItem
        {
            Item,
            BattleAction
        }
        
        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right
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
            Hold,
            MultiTap
        }

        public enum QTESuppress
        {
            None,
            Failure,
            Success,
        }

        public enum QTEDataUpdate
        {
            Preserve,
            ToHundred,
            ToZero
        }

        public enum Input
        {
            A,
            X,
            B,
            Y,
            Up,
            Down,
            Left,
            Right,
            Start,
            Select,
            LB,
            LT,
            RB,
            RT
        }

        public enum BattleActionEffect
        {
            Damage,
            Heal,
            Resurrection,
            StatusEffect,
            EffectsClear
        }
        
        public enum StatusEffectType
        {
            Buff,
            Debuff
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
        
        public enum StatusEffectExecute
        {
            Immediate,
            TurnStart,
            TurnEnd
        }
        
        public enum UnitStat
        {
            MaxHealth,
            Health,
            MaxEnergy,
            Energy,
            Strength,
            Focus,
            Vitality,
            Resistance,
            Agility,
            Accuracy,
            Luck,
            Initiative,
        }
        
        public enum StatModifierMultiplier
        {
            Add,
            Subtract,
            MultiplyPositive,
            MultiplyNegative,
            AddPercent,
            SubtractPercent
        }

        public enum SkillType
        {
            Active,
            Passive,
            Field
        }

        public enum FieldSkill
        {
            Cut,
            Teleport
        }
        
        public enum TargetSide
        {
            SameAsUnit,
            OppositeToUnit
        }
        
        public enum QTEOffset
        {
            RelativeToTarget,
            Absolute
        }

        public enum TargetsQuantity
        {
            Single,
            All
        }

        public enum TargetSelectionType
        {
            Standalone,
            WithOtherUIElements
        }

        public enum TargetSelectionFilter
        {
            All,
            Alive,
            Dead
        }

        public enum BattleState
        {
            Start,
            Win,
            Lose
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
        
        public enum BattleTeam
        {
            Party,
            Enemy
        }
        
        public enum AfterEventBehaviour
        {
            Destroy,
            Hide,
            Deactivate,
            RestoreAfterTime,
            RestoreImmediately
        }

        public enum TriggerPriority
        {
            Main,
            Sub
        }
        
        public enum TriggerActivationType
        {
            Manual,
            AutoEnter,
            AutoExit,
            Requirement
        }
        
        public enum TriggerLoopType
        {
            OneShot,
            Cycle
        }

        public enum TriggerActivationUponRequirementsMet
        {
            Immediate,
            Lazy
        }

        public enum StandardAnimation
        {
            Idle,
            Run,
            Attack,
            TakeDamage
        }
        
        public enum PortraitSide
        {
            Free,
            Left, 
            Right
        }

        public enum IconType
        {
            Portrait,
            Item,
            Skill
        }
    }
}