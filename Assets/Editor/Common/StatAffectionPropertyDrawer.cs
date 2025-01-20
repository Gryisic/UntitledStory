using System;
using Common.Models.Stats.Modifiers;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Common
{
    [CustomPropertyDrawer(typeof(StatAffection))]
    public class StatAffectionPropertyDrawer : PropertyDrawer
    {
        private Slider _borders;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(EditorPaths.PathToStatAffectionUXML);

            SerializedProperty multiplier = property.FindPropertyRelative(StatAffection.MultiplierPropertyName);
            
            visualTree.CloneTree(root);

            EnumField multiplierField = root.Q<EnumField>("Multiplier");
            _borders = root.Q<Slider>("Value");

            multiplierField.RegisterValueChangedCallback(ChangeBorders);
            
            ChangeBorders(multiplier.enumValueIndex);
            
            return root;
        }

        private void ChangeBorders(int index)
        {
            Enums.StatModifierMultiplier type = (Enums.StatModifierMultiplier) Enum.GetValues(typeof(Enums.StatModifierMultiplier)).GetValue(index);
            
            ChangeBorders(type);
        }
        
        private void ChangeBorders(ChangeEvent<Enum> type) => ChangeBorders((Enums.StatModifierMultiplier) type.newValue);

        private void ChangeBorders(Enums.StatModifierMultiplier type)
        {
            switch (type)
            {
                case Enums.StatModifierMultiplier.Add:
                    SetFlatValue();
                    break;
                
                case Enums.StatModifierMultiplier.Subtract:
                    SetFlatValue();
                    break;
                
                case Enums.StatModifierMultiplier.MultiplyPositive:
                    SetMultiplierValue();
                    break;
                
                case Enums.StatModifierMultiplier.MultiplyNegative:
                    SetMultiplierValue();
                    break;
                
                case Enums.StatModifierMultiplier.AddPercent:
                    SetPercentValue();
                    break;
                
                case Enums.StatModifierMultiplier.SubtractPercent:
                    SetPercentValue();
                    break;
            }
        }

        private void SetFlatValue()
        {
            _borders.lowValue = 0;
            _borders.highValue = 999;
        }
        
        private void SetMultiplierValue()
        {
            _borders.lowValue = 0;
            _borders.highValue = 2;
        }
        
        private void SetPercentValue()
        {
            _borders.lowValue = 0;
            _borders.highValue = 100;
        }
    }
}