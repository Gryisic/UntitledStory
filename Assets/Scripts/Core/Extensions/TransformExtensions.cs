using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Extensions
{
    public static class TransformExtensions
    {
        public static List<Transform> GetChilds(this Transform transform, bool includeNested = false)
        {
            List<Transform> childs = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                if (includeNested && child.transform.childCount > 0)
                {
                    List<Transform> nestedChilds = child.GetChilds(true);
                    
                    childs.AddRange(nestedChilds);
                }
                else
                {
                    childs.Add(child);
                }
            }

            return childs;
        }

        public static void ClampAround(this RectTransform transform, Vector2 position, Camera camera)
        {
            transform.position = position;
            
            if (IsFullyVisible(transform, camera))
                return;
            
            IReadOnlyList<Vector3> corners = GetCorners(transform);
            Vector3 bottomLeftCorner = camera.ViewportToWorldPoint(new Vector3(0, 0));
            Vector3 upperRightCorner = camera.ViewportToWorldPoint(new Vector3(1, 1));
            
            foreach (var corner in corners)
            {
                Vector3 centerPosition = transform.position;
                float horizontalOffset = centerPosition.x - corner.x;
                float verticalOffset = centerPosition.y - corner.y;
                    
                position.x = Mathf.Clamp(position.x, bottomLeftCorner.x + horizontalOffset, upperRightCorner.x - horizontalOffset);
                position.y = Mathf.Clamp(position.y, bottomLeftCorner.y + verticalOffset, upperRightCorner.y - verticalOffset);
            }

            transform.position = position;
        }

        public static bool IsVisible(this RectTransform rectTransform, Camera camera) 
            => CountOfVisibleCorners(rectTransform, camera) > 0;
        
        public static bool IsFullyVisible(this RectTransform rectTransform, Camera camera) 
            => CountOfVisibleCorners(rectTransform, camera) == 4;

        public static IReadOnlyList<Vector3> GetCorners(this RectTransform transform)
        {
            Vector3[] objectCorners = new Vector3[4];

            transform.GetWorldCorners(objectCorners);

            return objectCorners;
        }

        private static int CountOfVisibleCorners(RectTransform transform, Camera camera)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
            IReadOnlyList<Vector3> objectCorners = GetCorners(transform);

            return objectCorners.Select(camera.WorldToScreenPoint)
                .Count(localScreenSpaceCorner => screenBounds.Contains(localScreenSpaceCorner));
        }
    }
}