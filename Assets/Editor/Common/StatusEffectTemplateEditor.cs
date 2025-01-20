using System;
using Common.Models.StatusEffects;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Common
{
    [CustomEditor(typeof(StatusEffectTemplate))]
    public class StatusEffectTemplateEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        private EnumField _buffType;
        private EnumField _debuffType;
        
        private StatusEffectTemplate _template;
        
        public override VisualElement CreateInspectorGUI()
        {
            _template = target as StatusEffectTemplate;
            
            VisualElement root = new VisualElement();

            _treeAsset.CloneTree(root);

            EnumField type = root.Q<EnumField>("Type");
            
            _buffType = root.Q<EnumField>("BuffType");
            _debuffType = root.Q<EnumField>("DebuffType");
            
            type.RegisterValueChangedCallback(ToggleTypes);
            
            ToggleTypes(_template.Type);
            
            return root;
        }
        
        private void ToggleTypes(ChangeEvent<Enum> evt) => ToggleTypes((Enums.StatusEffectType)evt.newValue);

        private void ToggleTypes(Enums.StatusEffectType type)
        {
            switch (type)
            {
                case Enums.StatusEffectType.Buff:
                    Toggle(_buffType, _debuffType);
                    break;
                
                case Enums.StatusEffectType.Debuff:
                    Toggle(_debuffType, _buffType);
                    break;
            }
        }

        private void Toggle(EnumField activate, EnumField deactivate)
        {
            activate.style.display = DisplayStyle.Flex;
            deactivate.style.display = DisplayStyle.None;
        }
    }
}