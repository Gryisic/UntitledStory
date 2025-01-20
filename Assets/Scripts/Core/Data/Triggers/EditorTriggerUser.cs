using System;
using Core.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Data.Triggers
{
    [Serializable]
    public struct EditorTriggerUser
    {
        [SerializeField, HideInInspector] private int _sceneIndex;
        [SerializeField, HideInInspector] private string _sceneName;
        [SerializeField, HideInInspector] private string _objectName;
        [SerializeField, HideInInspector] private string _userID;
        [SerializeField, HideInInspector] private string _objectPath;

        public int SceneIndex => _sceneIndex;
        public string SceneName => _sceneName;
        public string ObjectName => _objectName;
        public string UserID => _userID;
        public string ObjectPath => _objectPath;


        public EditorTriggerUser(int sceneIndex, string sceneName, Object userObject, string userID)
        {
            _sceneIndex = sceneIndex;
            _sceneName = sceneName;
            _objectName = userObject.name;
            _userID = userID;
            
            GameObject gameObject = GameObject.Find(userObject.name);
            _objectPath = gameObject.transform.GetFullPath();
        }
    }
}