using System.IO;
using Common.Models.Skills;
using Common.Models.Skills.Templates;
using Common.QTE;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(ActiveSkillTemplate))]
    public class ActiveSkillTemplateEditor : UnityEditor.Editor
    {
        private const string QTESequenceFolderName = "QTESequence";
        
        private ActiveSkillTemplate _template;
        private UnityEditor.Editor _qteSequenceEditor;

        private Object _lastValidSequenceObject;

        private bool _isQTESequenceExpanded;

        private void OnEnable()
        {
            _template = (ActiveSkillTemplate) target;
            Object sequenceObject = serializedObject.FindProperty(ActiveSkillTemplate.QTESequencePropertyName).objectReferenceValue;

            if (sequenceObject) 
                _qteSequenceEditor = CreateEditor(sequenceObject);
        }

        private void OnDisable()
        {
            if (ReferenceEquals(_qteSequenceEditor, null) == false)
                DestroyImmediate(_qteSequenceEditor);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            serializedObject.Update();
            
            DrawPropertiesExcluding(serializedObject, ActiveSkillTemplate.QTESequencePropertyName);
            
            if (_template.HasQTE)
                DrawQTESequence();
            
            if (EditorGUI.EndChangeCheck())
            {
                ValidateQTESequence();

                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawQTESequence()
        {
            SerializedProperty qteSequenceProperty =
                serializedObject.FindProperty(ActiveSkillTemplate.QTESequencePropertyName);

            if (qteSequenceProperty.objectReferenceValue == null)
            {
                bool isSequenceExist = TryGetExistingSequence(out ScriptableObject sequence);
                
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Create New Sequence"))
                {
                    if (isSequenceExist)
                    {
                        if (EditorUtility.DisplayDialog("Attention!",
                                "This will delete existing sequence and creates a new one. Do you really want to proceed?",
                                    "Yes", "No"))
                        {
                            CreateSequence(qteSequenceProperty);
                        }
                    }
                    else
                    {
                        CreateSequence(qteSequenceProperty);
                    }
                }
                else if (isSequenceExist && GUILayout.Button("Select Existing Sequence"))
                {
                    qteSequenceProperty.objectReferenceValue = sequence;
                    
                    _qteSequenceEditor = CreateEditor(qteSequenceProperty.objectReferenceValue);
                }
                else
                {
                    if (ReferenceEquals(_qteSequenceEditor, null) == false)
                        DestroyImmediate(_qteSequenceEditor);
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                _isQTESequenceExpanded = EditorGUILayout.Foldout(_isQTESequenceExpanded, "QTE Sequence", true);

                EditorGUILayout.PropertyField(qteSequenceProperty, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }

            if (_isQTESequenceExpanded && ReferenceEquals(_qteSequenceEditor, null) == false)
                _qteSequenceEditor.OnInspectorGUI();
        }

        private void ValidateQTESequence()
        {
            SerializedProperty sequenceProperty = serializedObject.FindProperty(ActiveSkillTemplate.QTESequencePropertyName);
            string sequenceName = $"{serializedObject.targetObject.name}Sequence";

            if (sequenceProperty.objectReferenceValue != null &&
                ValidatePropertyName(sequenceProperty.objectReferenceValue.name, sequenceName) == false)
            {
                sequenceProperty.objectReferenceValue = _lastValidSequenceObject ? _lastValidSequenceObject : null;

                EditorUtility.DisplayDialog("Attention!",
                    "There's something wrong with the sequence name. Name of qte sequence should be written in the next format: 'ActionNameSequence'.",
                    "Got it!");
            }

            _lastValidSequenceObject = sequenceProperty.objectReferenceValue;
        }
        
        private void CreateSequence(SerializedProperty qteSequenceProperty)
        {
            qteSequenceProperty.objectReferenceValue = GetNewSequence();
            serializedObject.ApplyModifiedProperties();

            _qteSequenceEditor = CreateEditor(qteSequenceProperty.objectReferenceValue);

            AssetDatabase.SaveAssets();
        }

        private ScriptableObject GetNewSequence()
        {
            string path = GetPathToQTESequencesFolder();

            if (AssetDatabase.IsValidFolder(path) == false)
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            path = $"{path}/{serializedObject.targetObject.name}Sequence.asset";
            
            QuickTimeEventSequence sequence = CreateInstance<QuickTimeEventSequence>();
            AssetDatabase.CreateAsset(sequence, path);

            return sequence;
        }

        private bool TryGetExistingSequence(out ScriptableObject sequence)
        {
            string path = GetPathToQTESequencesFolder();
            
            path = $"{path}/{serializedObject.targetObject.name}Sequence.asset";
            sequence = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            return sequence;
        }

        private string GetPathToQTESequencesFolder()
        {
            string path = AssetDatabase.GetAssetPath(target);
            string fileName = Path.GetFileName(path);
            path = path.Replace(fileName, string.Empty);
            path = $"{path}/{QTESequenceFolderName}";

            return path;
        }

        private bool ValidatePropertyName(string currentName, string targetName) => currentName == targetName;
    }
}