using System.Collections.Generic;
using UnityEngine;

namespace Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static void Show(this GameObject gameObject) => gameObject.SetActive(true);
        
        public static void Hide(this GameObject gameObject) => gameObject.SetActive(false);

        public static List<GameObject> GetChilds(this GameObject gameObject, bool includeNested = false)
        {
            List<GameObject> childs = new List<GameObject>();

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;

                if (includeNested && child.transform.childCount > 0)
                {
                    List<GameObject> nestedChilds = child.GetChilds(true);
                    
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