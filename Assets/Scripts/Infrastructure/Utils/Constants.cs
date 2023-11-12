﻿namespace Infrastructure.Utils
{
    public static class Constants
    {
        public const int StartSceneIndex = 0;
        public const int FloorLayerIndex = 7;
        public const int SafeNumberOfStepsInLoops = 500;

        public const int ActivatedCameraPriority = 100;
        public const int DeactivatedCameraPriority = 0;
        
        public const float NeutralFollowCameraSize = 5f;
        public const float CloseFollowCameraSize = 3.5f;
        public const float FarFollowCameraSize = 7.5f;
        
        public const float NeutralFocusCameraSize = 5f;
        public const float CloseFocusCameraSize = 3.5f;
        public const float FarFocusCameraSize = 7f;

        public const float DefaultCameraBlendTime = 0.2f;

        public const int InitialCopiesOfUnit = 5;
        public const int InitialCopiesOfProjectiles = 15;

        public const int DefaultStatMultiplier = 1;
        
        public const int MaxStatValue = 9999;
        public const int MinStatValue = 0;

        public const float DialogueLetterPrintTime = 0.1f;
        public const float AutoDialogueModeAwaitTime = 1f;
        
        public const float InteractionRadius = 1f;
        public const float ExplorationMovementSpeed = 5f;
        public const float FallMultiplier = 3f;
        public const float LinearVelocitySlowdownSpeed = 0.9f;
        public const float DashRefreshingTime = 2f;
        public const float JumpForce = 15f;
        public const float DropThroughPlatformTime = 0.25f;
        
        public const float ProjectileDangerZoneTime = 0.75f;

        public const float DefaultLegacyUnitSpawnDistance = 5f;

        public const float DefaultEnemyAwaitTime = 2f;

        public const float UnitSelectionCardHorizontalOffset = 4.7f;
        public const float UnitSelectionCardPortraitVerticalOffset = 1.25f;

        public const float StorageSpinPrewarmTime = 1f;
        public const float StorageSpinTime = 3f;
        public const float StorageSpinAfterimageTime = 1f;
        
        public const float ModifierCardHoverVerticalOffset = 110;
        public const float ModifierCardActivationVerticalOffset = 810;

        public const string PathToUnitPrefabs = "Units";
        public const string PathToProjectilesPrefabs = "Projectiles";
        public const string PathToDialogueAssets = "Dialogues";
        
        public const string ShaderHealthValueReference = "_HealthValue";
        public const string DissolveValueReference = "_DissolvePercent";
    }
}