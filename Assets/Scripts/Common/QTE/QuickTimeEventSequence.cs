using System.Collections.Generic;
using System.Linq;
using Common.QTE.Templates;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.QTE
{
    public class QuickTimeEventSequence : ScriptableObject
    {
        [SerializeField, Expandable] private List<QuickTimeEventTemplate> _templates;

        private IReadOnlyList<QuickTimeEvent> _events;

#if UNITY_EDITOR
        public static string SequenceListPropertyName => nameof(_templates);
#endif

        public IReadOnlyList<QuickTimeEvent> GetEvents()
        {
            if (_events is not { Count: > 0 }) 
                CreateEvents();

            return _events;
        }

        private void CreateEvents() => _events = _templates.Select(template => new QuickTimeEvent(template)).ToList();
    }
}