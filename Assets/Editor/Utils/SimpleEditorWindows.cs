using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils
{
    public static class SimpleEditorWindows
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

        public static void DrawSimpleDropdownButton(Rect position, string name, IEnumerable collection, string onComparer, GenericMenu.MenuFunction callback)
        {
            Rect dropdownRect = position;

            dropdownRect.x += 15;
            dropdownRect.width -= 15;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUI.DropdownButton(dropdownRect, new(name), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                foreach (string value in collection)
                {
                    menu.AddItem(new GUIContent(value), value == onComparer, callback);
                }
                
                menu.ShowAsContext();
            }
        }
    }
}