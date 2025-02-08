using System;
using Common.Models.GameEvents.General;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Core.Data.Events
{
    [Serializable, CreateAssetMenu(menuName = "Core/Data/Events/Template")]
    public class EventData : ScriptableObject, IDisposable
    {
        [SerializeField, AsFileName] private string _id;
        [SerializeField, EditableIfAsset] private bool _isActive;
        [SerializeReference, SubclassesPicker(disableManualChange: true)] private GeneralEvent _event;

        public string ID => _id;
        public bool IsActive => _isActive;
        public GeneralEvent Event => _event;

        public void Activate() => _isActive = true;
        
        public void Deactivate() => _isActive = false;

        public void Dispose() => _event?.Dispose();
    }
}