using System;
using System.Collections.Generic;
using Core.Data.Triggers;
using Editor.Utils;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Triggers
{
    [CustomPropertyDrawer(typeof(EditorTrigger))]
    public class EditorTriggerPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorPaths.PathToEditorTriggerUXML);
            List<EditorTriggerUser> users = GetUsersFromProperty(property);
            
            visualTree.CloneTree(root);
            
            Foldout foldout = root.Q<Foldout>("foldout");
            foldout.text = property.FindPropertyRelative(EditorTrigger.IDPropertyName).stringValue;
            
            CreateUsers(root, users, property);
            
            return root;
        }

        private void CreateUsers(VisualElement root, List<EditorTriggerUser> users, SerializedProperty property)
        {
            Func<VisualElement> makeUser = () =>
            {
                VisualElement root = new VisualElement();
                VisualTreeAsset visualTree =
                    AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorPaths.PathToEditorTriggeUserUXML);

                visualTree.CloneTree(root);
                
                return root;
            };

            Action<VisualElement, int> bindItem = (element, index) =>
            {
                Label userLabel = element.Q<Label>("user_label");
                Label sceneLabel = element.Q<Label>("scene_label");
                Button toUserButton = element.Q<Button>();
                
                sceneLabel.text = $"In {users[index].SceneName}";
                userLabel.text = users[index].ObjectName;
                toUserButton.text = "To User";
                
                toUserButton.RegisterCallback<ClickEvent>(evt =>
                {
                    bool onScene = EditorScenesUtils.ToSceneByIndexWithPing(users[index].SceneIndex, users[index].ObjectPath);
                    
                    if (onScene) 
                        return;
                    
                    if (EditorUtility.DisplayDialog("User not found",
                            $"Cannot find an object {users[index].ObjectName} on scene {ScenesUtils.GetNameByIndex(users[index].SceneIndex)} at path {users[index].ObjectPath}",
                            "Remove from users!", "Preserve"))
                    {
                        EditorTrigger trigger = (EditorTrigger) EditorUtils.GetTargetObjectOfProperty(property);
                        
                        trigger.RemoveUser(users[index].UserID);
                    }
                });
            };

            ListView usersList = root.Q<ListView>();
            
            usersList.makeItem = makeUser;
            usersList.bindItem = bindItem;
        }

        private List<EditorTriggerUser> GetUsersFromProperty(SerializedProperty property)
        {
            SerializedProperty usersProperty = property.FindPropertyRelative(EditorTrigger.UsersPropertyName);
            List<EditorTriggerUser> users = new List<EditorTriggerUser>();

            for (int i = 0; i < usersProperty.arraySize; i++)
            {
                SerializedProperty element = usersProperty.GetArrayElementAtIndex(i);
                EditorTriggerUser user = (EditorTriggerUser) EditorUtils.GetTargetObjectOfProperty(element);
                
                users.Add(user);
            }

            return users;
        }
    }
}