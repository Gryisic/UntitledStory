#if UNITY_EDITOR
using Core.Extensions;
using Editor.Utils;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(AsFileNameAttribute))]
    public class AsFileNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string name = property.serializedObject.targetObject.name;
            property.stringValue = name;

            float fileNameWidth = position.width * 0.625f;
            float propertyNameWidth = position.width - fileNameWidth;

            position.width = propertyNameWidth;
            
            EditorGUI.LabelField(position, property.name.ToFirstUpper(true));

            position.width = fileNameWidth;
            
            SimpleEditorWindows.DrawInformationBox(position, fileNameWidth, propertyNameWidth, property.stringValue);
        }
    }
}
#endif