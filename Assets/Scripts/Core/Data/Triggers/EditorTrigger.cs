using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.General;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Data.Triggers
{
    [Serializable]
    public class EditorTrigger : ITrigger
    {
        [SerializeField] private string _id;
        [SerializeReference, SubclassesPicker(false)] private GeneralEvent _event;
        [SerializeField] private List<EditorTriggerUser> _users;
        [SerializeField] private bool _isActive;
        [SerializeField] private Enums.TriggerActivationType _activationType;
        [SerializeField] private Enums.TriggerLoopType _loopType;
        
        public Enums.PostEventState PostEventState { get; }
            
        public string ID => _id;
        public string SourceName => string.Empty;
        public string Type => string.Empty;
        public bool IsActive => _isActive;
        public Enums.TriggerActivationType ActivationType => _activationType;
        public Enums.TriggerLoopType LoopType => _loopType;
        public IGameEvent Event => _event;

        public event Action<IGameEvent> Ended;

        public void Activate() => _isActive = true;
            
        public void Deactivate() => _isActive = false;
        
#if UNITY_EDITOR
        public static string IDPropertyName => nameof(_id);
        public static string UsersPropertyName => nameof(_users);

        public void AddUser(string userID, Object gameObject)
        {
            if (_users.Exists(u => u.UserID == userID))
                return;
            
            Debug.Log($"Add: {userID}");
            
            int sceneIndex = ScenesUtils.GetActiveSceneIndex();
            string sceneName = ScenesUtils.GetActiveSceneName();
            EditorTriggerUser user = new EditorTriggerUser(sceneIndex, sceneName, gameObject, userID);
            
            _users.Add(user);
        }
        
        public void RemoveUser(string userID)
        {
            Debug.Log($"Remove: {userID}");
            
            if (_users.Exists(u => u.UserID == userID) == false)
                return;

            EditorTriggerUser user = _users.First(u => u.UserID == userID);

            _users.Remove(user);
        }
#endif
    }
}