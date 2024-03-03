using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Infrastructure.Utils
{
    public static class DebugSprites
    {
        private const string PathToDebugData = "Assets/Data/Debug/Sprites/";
        
        public static Sprite GetHero() => 
            AssetDatabase.LoadAssetAtPath<Sprite>($"{PathToDebugData}Hero.png");
        
        public static Sprite GetEnemy() => 
            AssetDatabase.LoadAssetAtPath<Sprite>($"{PathToDebugData}Enemy.png");
        
        public static Sprite GetInputMarker() => 
            AssetDatabase.LoadAssetAtPath<Sprite>($"{PathToDebugData}InputReleased.png");
    }
}
#endif