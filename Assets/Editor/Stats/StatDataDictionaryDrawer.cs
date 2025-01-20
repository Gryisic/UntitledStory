using Common.Models.Stats;
using Editor.GenericDrawers;
using Editor.Utils;
using Infrastructure.Utils;
using Infrastructure.Utils.Types;
using UnityEditor;
using UnityEngine;

namespace Editor.Stats
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<Enums.UnitStat, StatTemplate>))]
    public class StatDataDictionaryDrawer : SerializableDictionaryDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            base.OnGUI(rect, prop, label);

            SuppressAddButton();
            SuppressRemoveButton();

            if (prop.isExpanded == false)
                return;
            
            DrawHelperData();
        }

        private void DrawHelperData()
        {
            Rect rect = GUILayoutUtility.GetLastRect();

            float keyWidth = rect.width;
            keyWidth *= 0.25f;

            float valueWidth = rect.width - keyWidth;
            float partWidth = valueWidth / 8;
            float dataWidth = partWidth * 6;

            rect.x += keyWidth + 12;
            rect.height = EditorGUIUtility.singleLineHeight;

            SimpleEditorWindows.DrawInformationBox(rect, partWidth - 2, 0, "Initial Value");
            SimpleEditorWindows.DrawInformationBox(rect, partWidth - 2, partWidth + 3, "Growth Modifier");
            SimpleEditorWindows.DrawInformationBox(rect, dataWidth - 10, partWidth * 2 + 6, "Level Data");
        }
    }
}