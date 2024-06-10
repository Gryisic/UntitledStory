using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class CameraExtensions
    {
        private const float DefaultAspectResolution = 1.7f;
        
        private static readonly Dictionary<float, ResolutionMap> ResolutionMaps = new()
        {
            { 1.3f, new ResolutionMap(4, 3) },
            { 1.7f, new ResolutionMap(16, 9) },
            { 1.6f, new ResolutionMap(16, 10) },
            { 2.3f, new ResolutionMap(21, 9) }
        };

        public static void GetResolutionSteps(this Camera camera, out float width, out float height)
        {
            float aspect = (float) Math.Round(camera.aspect, 1, MidpointRounding.AwayFromZero);
            ResolutionMap map = ResolutionMaps.TryGetValue(aspect, out ResolutionMap resolutionMap) ? resolutionMap : ResolutionMaps[DefaultAspectResolution];

            width = Screen.width / map.Width;
            height = Screen.height / map.Height;
        }

        public static Vector2 GetScreenCenter(this Camera camera)
        {
            float width = Screen.width / 2;
            float height = Screen.height / 2;

            return new Vector2(width, height);
        }
        
        private struct ResolutionMap
        {
            public float Width { get; }
            public float Height { get; }
            
            public ResolutionMap(float width, float height)
            {
                Width = width;
                Height = height;
            }
        }
    }
}