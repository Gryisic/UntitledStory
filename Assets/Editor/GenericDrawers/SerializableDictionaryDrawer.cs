#if UNITY_EDITOR
using System.Collections.Generic;
using Editor.Utils;
using Infrastructure.Utils;
using Infrastructure.Utils.Types;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private SerializedProperty _pairsList;
        private SerializedProperty _dividerPosProp;
        
        private ReorderableList _reorderableList;
        private bool _isDividerDragged;
        
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            var indentedRect = EditorGUI.IndentedRect(rect);

            void Head()
            {
                var headerRect = indentedRect;
                headerRect.height = EditorGUIUtility.singleLineHeight;

                void ExpandablePanel()
                {
                    var fullHeaderRect = new Rect(headerRect);
                    fullHeaderRect.x -= 17;
                    fullHeaderRect.width += 34;

                    if (Event.current != null && fullHeaderRect.Contains(Event.current.mousePosition))
                    {
                        Color transparentGrey = new Color(0.4f, 0.4f, 0.4f, 0.4f);
                        EditorGUI.DrawRect(fullHeaderRect, transparentGrey);
                    }

                    GUI.color = Color.clear;

                    if (GUI.Button(new Rect(fullHeaderRect.x, fullHeaderRect.y, fullHeaderRect.width - 40,
                                            fullHeaderRect.height), ""))
                    {
                        prop.isExpanded = !prop.isExpanded;
                    }

                    GUI.color = Color.white;

                    var triangleRect = rect;
                    triangleRect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUI.Foldout(triangleRect, prop.isExpanded, "");
                }

                void DisplayName()
                {
                    GUI.color = Color.white;
                    GUI.Label(headerRect, prop.displayName);
                    GUI.color = Color.white;
                    GUI.skin.label.fontSize = 12;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                }

                void DuplicatedKeysWarning()
                {
                    if (Event.current != null && Event.current.type != EventType.Repaint)
                    {
                        return;
                    }

                    var hasRepeated = false;
                    var repeatedKeys = new List<string>();

                    for (int i = 0; i < _pairsList.arraySize; i++)
                    {
                        SerializedProperty isKeyRepeatedProperty = _pairsList.GetArrayElementAtIndex(i)
                                                                           .FindPropertyRelative("isKeyDuplicated");

                        if (isKeyRepeatedProperty.boolValue)
                        {
                            hasRepeated = true;
                            SerializedProperty keyProperty = _pairsList.GetArrayElementAtIndex(i).FindPropertyRelative("Key");
                            string keyString = GetSerializedPropertyValueAsString(keyProperty);
                            repeatedKeys.Add(keyString);
                        }
                    }

                    if (!hasRepeated)
                    {
                        return;
                    }

                    float with = GUI.skin.label.CalcSize(new GUIContent(prop.displayName)).x;
                    headerRect.x += with + 24f;
                    var warningRect = headerRect;
                    Rect warningRectIcon = new Rect(headerRect.x - 18, headerRect.y, headerRect.width, headerRect.height);
                    GUI.color = Color.white;
                    GUI.Label(warningRectIcon, EditorGUIUtility.IconContent("console.erroricon"));
                    GUI.color = new Color(1.0f, 0.443f, 0.443f);
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.Label(warningRect, "Duplicated keys: " + string.Join(", ", repeatedKeys));
                    GUI.color = Color.white;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                }

                string GetSerializedPropertyValueAsString(SerializedProperty property)
                {
                    switch (property.propertyType)
                    {
                        case SerializedPropertyType.Integer:
                            return property.intValue.ToString();
                        case SerializedPropertyType.Boolean:
                            return property.boolValue.ToString();
                        case SerializedPropertyType.Float:
                            return property.floatValue.ToString();
                        case SerializedPropertyType.String:
                            return property.stringValue;
                        default:
                            return "(Unsupported Type)";
                    }
                }

                ExpandablePanel();
                DisplayName();
                DuplicatedKeysWarning();
            }

            void List()
            {
                if (!prop.isExpanded)
                {
                    return;
                }

                SetupList(prop);

                float newHeight = indentedRect.height - EditorGUIUtility.singleLineHeight - 3;
                indentedRect.y += indentedRect.height - newHeight;
                indentedRect.height = newHeight;

                _reorderableList.DoList(indentedRect);
            }

            SetupProps(prop);

            Head();
            List();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            SetupProps(prop);

            var height = EditorGUIUtility.singleLineHeight;

            if (prop.isExpanded)
            {
                SetupList(prop);
                height += _reorderableList.GetHeight() + 5;
            }

            return height;
        }

        protected void SuppressAddButton()
        {
            if (_reorderableList != null)
                _reorderableList.displayAdd = false;
        }

        protected void SuppressRemoveButton()
        {
            if (_reorderableList != null)
                _reorderableList.displayRemove = false;
        }
        
        private void SetupList(SerializedProperty prop)
        {
            if (_reorderableList != null)
            {
                return;
            }

            SetupProps(prop);

            _reorderableList = new ReorderableList(_pairsList.serializedObject, _pairsList, true, false, true, true);
            _reorderableList.drawElementCallback = DrawListElement;
            _reorderableList.elementHeightCallback = GetListElementHeight;
            _reorderableList.drawNoneElementCallback = ShowDictIsEmptyMessage;
        }

        private void SetupProps(SerializedProperty prop)
        {
            if (_property != null)
                return;

            _property = prop;
            _pairsList = prop.FindPropertyRelative("dictionaryList");
            _dividerPosProp = prop.FindPropertyRelative("dividerPos");
        }

        private float GetListElementHeight(int index)
        {
            var kvpProp = _pairsList.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            float GetPropertyHeight(SerializedProperty prop)
            {
                if (IsSingleLine(prop))
                {
                    return EditorGUI.GetPropertyHeight(prop);
                }

                var height = 1f;

                foreach (var childProp in GetChildren(prop, false))
                {
                    height += EditorGUI.GetPropertyHeight(childProp) + 1;
                }

                height += 10;

                return height;
            }

            return Mathf.Max(GetPropertyHeight(keyProp), GetPropertyHeight(valueProp));
        }

        void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect keyRect;
            Rect valueRect;
            Rect dividerRect;

            var kvpProp = _pairsList.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            void Draw(Rect rect, SerializedProperty prop)
            {
                if (IsSingleLine(prop))
                {
                    rect.height = EditorGUIUtility.singleLineHeight;
                    
                    EditorGUI.PropertyField(rect, prop, GUIContent.none);
                }
                else
                {
                    foreach (var childProp in GetChildren(prop, false))
                    {
                        float childPropHeight = EditorGUI.GetPropertyHeight(childProp);
                        
                        rect.height = childPropHeight;
                        EditorGUI.PropertyField(rect, childProp, true);
                        rect.y += childPropHeight + 2;
                    }
                }
            }

            void DrawRects()
            {
                var dividerWidh = IsSingleLine(valueProp) ? 6 : 16f;
                var dividerPosition = 0.25f;

                var fullRect = rect;
                fullRect.width -= 1;
                fullRect.height -= 2;
                
                keyRect = fullRect;
                valueRect = fullRect;

                if (IsPropertyUntypical(keyProp))
                {
                    keyRect.width = fullRect.width / 2;

                    valueRect.x += keyRect.width + 5;
                    valueRect.width = keyRect.width;
                }
                else
                {
                    keyRect.width *= dividerPosition;
                    keyRect.width -= dividerWidh / 2;
                
                    valueRect.x += fullRect.width * dividerPosition;
                    valueRect.width *= (1 - dividerPosition);
                    valueRect.width -= dividerWidh / 2;
                }
                
                dividerRect = fullRect;
                dividerRect.x += fullRect.width * dividerPosition - dividerWidh / 2;
                dividerRect.width = dividerWidh;
            }

            void Key()
            {
                Draw(keyRect, keyProp);

                if (kvpProp.FindPropertyRelative("isKeyDuplicated").boolValue)
                {
                    GUI.Label(new Rect(keyRect.x + keyRect.width - 20, keyRect.y - 1, 20, 20),
                              EditorGUIUtility.IconContent("console.erroricon"));
                }
            }

            void Value()
            {
                Draw(valueRect, valueProp);
            }

            void Divider()
            {
                EditorGUIUtility.AddCursorRect(dividerRect, MouseCursor.ResizeHorizontal);

                if (Event.current == null || rect.Contains(Event.current.mousePosition) == false)
                {
                    return;
                }

                if (Event.current != null && dividerRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        _isDividerDragged = true;
                    }
                    else if (Event.current.type == EventType.MouseUp
                             || Event.current.type == EventType.MouseMove
                             || Event.current.type == EventType.MouseLeaveWindow)
                    {
                        _isDividerDragged = false;
                    }
                }

                if (_isDividerDragged && Event.current != null && Event.current.type == EventType.MouseDrag)
                {
                    _dividerPosProp.floatValue = Mathf.Clamp(_dividerPosProp.floatValue + Event.current.delta.x / rect.width, .2f, .8f);
                }
            }

            DrawRects();
            Key();
            Value();
            Divider();
        }

        private void ShowDictIsEmptyMessage(Rect rect)
        {
            GUI.Label(rect, "Empty");
        }

        private IEnumerable<SerializedProperty> GetChildren(SerializedProperty prop, bool enterVisibleGrandchildren)
        {
            prop = prop.Copy();

            var startPath = prop.propertyPath;

            var enterVisibleChildren = true;

            while (prop.NextVisible(enterVisibleChildren) && prop.propertyPath.StartsWith(startPath))
            {
                yield return prop;
                enterVisibleChildren = enterVisibleGrandchildren;
            }
        }

        private bool IsSingleLine(SerializedProperty prop)
        {
            return prop.propertyType != SerializedPropertyType.Generic || prop.hasVisibleChildren == false;
        }

        private bool IsPropertyUntypical(SerializedProperty property)
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Generic => true,
                _ => false
            };
        }
    }
}
#endif