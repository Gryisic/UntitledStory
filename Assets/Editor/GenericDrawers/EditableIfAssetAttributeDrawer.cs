using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(EditableIfAssetAttribute))]
    public class EditableIfAssetAttributeDrawer : PropertyDrawer
    {
        private bool _isEditingAsset;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _isEditingAsset = AssetDatabase.Contains(property.serializedObject.targetObject);

            if (_isEditingAsset == false)
                label.tooltip = "This field can only be edited in an asset";

            EditorGUI.BeginDisabledGroup(_isEditingAsset == false);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => EditorGUI.GetPropertyHeight(property, label, true);
    }
}