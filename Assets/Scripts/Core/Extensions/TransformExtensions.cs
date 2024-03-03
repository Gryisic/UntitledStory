using System.Collections.Generic;
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
    }
}