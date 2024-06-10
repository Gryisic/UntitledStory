using UnityEngine;

namespace Editor.Utils
{
    public static class SimpleEditorBoxes
    {
        public static void DrawInformationBox(Rect rect, float width, float offset, GUIContent guiContent)
        {
            Color defaultColor = GUI.backgroundColor;
            Color defaultContentColor = GUI.contentColor;
            
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            
            rect.x += offset;
            rect.width = width;

            GUI.Box(rect, guiContent);

            GUI.backgroundColor = defaultColor;
            GUI.contentColor = defaultContentColor;
        }

        public static void DrawInformationBox(Rect rect, float width, float offset, string text) 
            => DrawInformationBox(rect, width,offset,new GUIContent(text));
    }
}