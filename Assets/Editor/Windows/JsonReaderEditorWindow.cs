#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class JsonReaderEditorWindow : EditorWindow
    {
        private const string PathToVisualAsset = "Assets/Editor/UXML/ReadJsonUXML.uxml";
        private const string PathToValueVisualAsset = "Assets/Editor/UXML/JsonValueVisualElementUXML.uxml";
        private const string PathToArrayVisualAsset = "Assets/Editor/UXML/JsonArrayVisualElementUXML.uxml";
        private const string PathToHashtableVisualAsset = "Assets/Editor/UXML/JsonHashtableVisualElementUXML.uxml";
        private const string PathToJsonFiles = "Localization/Json/";
        
        private ListView _listView;
        private Button _refreshButton;

        [MenuItem("Tools/Json Reader")]
        public static void Open()
        {
            JsonReaderEditorWindow window = GetWindow<JsonReaderEditorWindow>();

            window.titleContent = new GUIContent("Json Reader");
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathToVisualAsset);
            List<TextAsset> jsons = GetJsons();
            VisualElement tree = visualTree.Instantiate();

            Func<VisualElement> makeItem = () =>
            {
                VisualElement localRoot = new VisualElement();
                Foldout foldout = new Foldout();
                Button copyButton = GetCloneButton(localRoot);
                
                localRoot.Add(foldout);
                localRoot.Add(copyButton);

                return localRoot;
            };

            Action<VisualElement, int> bindItem = (element, index) =>
            {
                Foldout foldout = element.Q<Foldout>();
                string text = jsons[index].text;

                foldout.text = jsons[index].name.WithSpaces();

                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(text);
                JsonHashtableVisualElement parsedElement = Extensions.GetVisualElement<JsonHashtableVisualElement>();
                
                foldout.Add(parsedElement);
                
                ProcessJObject(jsonObject, parsedElement);
            };

            root.Add(tree);

            _listView = tree.Q<ListView>();

            _listView.itemsSource = jsons;
            _listView.makeItem = makeItem;
            _listView.bindItem = bindItem;

            _refreshButton = tree.Q<Button>();
            
            _refreshButton.RegisterCallback<ClickEvent>(evt =>
            {
                _listView.Rebuild();
            });
        }

        #region Processors

        private void ProcessJObject(JObject jObject, JsonFieldVisualElement visualElement)
        {
            string text = string.Empty;
            int number;

            foreach (var jProperty in jObject.Properties())
            {
                if (TryConvertTo(jProperty, out text) || TryConvertTo(jProperty, out number))
                {
                    JsonValueVisualElement valueVisualElement = new JsonValueVisualElement();

                    valueVisualElement.Key = jProperty.Name;
                    valueVisualElement.Value = text;
                    
                    if (visualElement is JsonHashtableVisualElement hashtableVisualElement)
                        hashtableVisualElement.SetValueElement(valueVisualElement);
                    
                    visualElement.Add(valueVisualElement);
                    
                    continue;
                }

                if (TryGetJContainer(jProperty, out JObject newObject))
                {
                    JsonHashtableVisualElement hashElement = Extensions.ResolveVisualElements<JsonHashtableVisualElement>(visualElement);

                    hashElement.SetValueElement(hashElement);
                    
                    ProcessJObject(newObject, hashElement);
                }
                else if (TryGetJContainer(jProperty, out JArray jArray))
                {
                    JsonArrayVisualElement arrayElement = Extensions.ResolveVisualElements<JsonArrayVisualElement>(visualElement);
                    
                    if (visualElement is JsonHashtableVisualElement hashtableVisualElement) 
                        hashtableVisualElement.SetValueElement(arrayElement);
                    
                    ProcessJArray(jArray, arrayElement, jProperty.Name);
                }
            }
        }

        private void ProcessJArray(JArray jArray, JsonArrayVisualElement arrayVisualElement, string key)
        {
            try
            {
                foreach (var jObject in jArray.Values<JObject>())
                {
                    JsonHashtableVisualElement hashElement = Extensions.ResolveVisualElements<JsonHashtableVisualElement>(arrayVisualElement);
                    
                    arrayVisualElement.SetExplicitValueElement(key, hashElement);
                    
                    ProcessJObject(jObject, hashElement);
                }
            }
            catch
            {
                IEnumerator<JToken> enumerator = jArray.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    JToken jToken = enumerator.Current;

                    if (TryConvertTo(jToken, out string text))
                    {
                        JsonValueVisualElement valueVisualElement = new JsonValueVisualElement();
                        
                        arrayVisualElement.SetValueElement(valueVisualElement);
                        
                        valueVisualElement.Key = key;
                        valueVisualElement.Value = text;
                    }
                    else if (TryConvertTo(jToken, out JObject jObject))
                    {
                        JsonHashtableVisualElement hashElement = Extensions.ResolveVisualElements<JsonHashtableVisualElement>(arrayVisualElement);
                        
                        arrayVisualElement.SetExplicitValueElement(key, hashElement);
                        
                        ProcessJObject(jObject, hashElement);
                    }
                    else if (TryConvertTo(jToken, out JArray newArray))
                    {
                        JsonArrayVisualElement arrayElement = Extensions.ResolveVisualElements<JsonArrayVisualElement>(arrayVisualElement);
                        
                        ProcessJArray(newArray, arrayElement, key);
                    }
                }
            }
        }

        #endregion

        #region Converters

        private bool TryGetJContainer<T>(JProperty jProperty, out T jObject) where T : JContainer
        {
            try
            {
                jObject = JsonConvert.DeserializeObject<T>(jProperty.Value.ToString());
                return true;
            }
            catch
            {
                jObject = null;
                return false;
            }
        }

        private bool TryConvertTo<T>(JToken token, out T newObject)
        {
            try
            {
                newObject = token.ToObject<T>();
                return true;
            }
            catch
            {
                newObject = default;
                return false;
            }
        }

        private bool TryConvertTo<T>(JProperty property, out T newObject)
        {
            try
            {
                newObject = property.ToObject<T>();
                return true;
            }
            catch
            {
                newObject = default;
                return false;
            }
        }

        #endregion

        #region Utils

        private List<TextAsset> GetJsons() => Resources.LoadAll<TextAsset>(PathToJsonFiles).ToList();

        private Button GetCloneButton(VisualElement localRoot)
        {
            Button button = new Button();

            button.text = "Clone";
            
            button.RegisterCallback<ClickEvent, VisualElement>(CloneTree, localRoot);
            
            return button;
        }

        private void CloneTree(ClickEvent evt, VisualElement localRoot)
        {
            JsonHashtableVisualElement hashtable = localRoot.Q<JsonHashtableVisualElement>();
            JsonArrayVisualElement value = hashtable.Value as JsonArrayVisualElement;
            JsonFieldVisualElement copy = value.GetCopy(hashtable);
            
            //Debug.Log(copy.Children().First());
            
            Debug.Log(hashtable.childCount);
            
            hashtable.Add(copy);
            
            Debug.Log(hashtable.childCount);
        }

        #endregion

        #region JsonElements

        private abstract class JsonFieldVisualElement : VisualElement
        {
            public abstract JsonFieldVisualElement GetCopy(JsonFieldVisualElement parent);
        }
        
        private class JsonValueVisualElement : JsonFieldVisualElement
        {
            private readonly TextField _keyField;
            private readonly TextField _valueField;
            
            public string Key
            {
                get => _keyField.value;
                set => _keyField.value = value;
            }
            
            public string Value
            {
                get => _valueField.value;
                set => _valueField.value = value;
            }

            public JsonValueVisualElement()
            {
                VisualElement root = Extensions.TypeMap[JsonVisualElementType.Value].Instantiate();

                _keyField = root.Q<TextField>("Key");
                _valueField = root.Q<TextField>("Value");
                
                Add(root);
            }
            
            public void SuspendKey()
            {
                _keyField.style.display = DisplayStyle.None;
                _valueField.style.marginLeft = new StyleLength(new Length(51, LengthUnit.Percent));
            }

            public override JsonFieldVisualElement GetCopy(JsonFieldVisualElement parent)
            {
                JsonValueVisualElement copy = new JsonValueVisualElement();

                copy.Value = Value;
                
                return copy;
            }
        }

        private class JsonHashtableVisualElement : JsonFieldVisualElement
        {
            private readonly VisualElement _root;
            private readonly VisualElement _container;
            
            public JsonFieldVisualElement Value { get; private set; }

            public JsonHashtableVisualElement()
            {
                _root = Extensions.TypeMap[JsonVisualElementType.Hashtable].Instantiate();
                
                _container = _root.Q<VisualElement>("Container");
                
                Add(_container);
            }

            public void SetValueElement(JsonVisualElementType type)
            {
                JsonFieldVisualElement valueField = Extensions.DefineElement(type, this);
                
                Value = valueField;
                
                DropdownField dropdownField = _container.Q<DropdownField>("ValueSelector");
                
                dropdownField.style.display = DisplayStyle.None;
            }
            
            public void SetValueElement(JsonFieldVisualElement element)
            {
                Value = element;
                
                DropdownField dropdownField = _container.Q<DropdownField>("ValueSelector");

                dropdownField.style.display = DisplayStyle.None;
            }

            public override JsonFieldVisualElement GetCopy(JsonFieldVisualElement parent)
            {
                JsonHashtableVisualElement copy = new JsonHashtableVisualElement();

                copy.Value = Value;
                
                return copy;
            }
        }
        
        private class JsonArrayVisualElement: JsonFieldVisualElement
        {
            private readonly VisualElement _root;
            
            private TextField _keyElement;
            private List<string> _keys;

            private List<JsonFieldVisualElement> _childs;
            
            public string Key
            {
                get => _keyElement?.value;
                set
                {
                    if (ReferenceEquals(_keyElement, null))
                        return;
                    
                    _keyElement.value = value;
                }
            }

            public JsonArrayVisualElement()
            {
                _root = Extensions.TypeMap[JsonVisualElementType.Array].Instantiate();
                _keys = new List<string>();
                _childs = new List<JsonFieldVisualElement>();
                
                Add(_root);
            }
            
            public void SetValueElement(JsonVisualElementType type)
            {
                JsonFieldVisualElement valueField = Extensions.DefineElement(type, this);
                
                _root.Add(valueField);
                _childs.Add(valueField);

                if (valueField is JsonValueVisualElement valueVisualElement)
                {
                    string key = valueVisualElement.Key;

                    if (_keys.Contains(key))
                        valueVisualElement.SuspendKey();
                    else
                        _keys.Add(key);
                }

                if (type == JsonVisualElementType.Hashtable && _keyElement == null)
                {
                    _keyElement = new TextField();
                    
                    _root.Insert(0, _keyElement);
                }
            }

            public void SetValueElement(JsonFieldVisualElement element)
            {
                _root.Add(element);
                _childs.Add(element);
                
                if (element is JsonValueVisualElement valueVisualElement)
                {
                    string key = valueVisualElement.Key;

                    if (_keys.Contains(key))
                        valueVisualElement.SuspendKey();
                    else
                        _keys.Add(key);
                }
                
                DropdownField dropdownField = _root.Q<DropdownField>("ValueSelector");

                dropdownField.style.display = DisplayStyle.None;
            }
            
            public void SetExplicitValueElement(string key, JsonFieldVisualElement element)
            {
                if (_keyElement == null)
                {
                    _keyElement = new TextField();

                    _root.Insert(0, _keyElement);

                    _keyElement.value = key;
                }
                
                SetValueElement(element);
            }

            public override JsonFieldVisualElement GetCopy(JsonFieldVisualElement parent)
            {
                JsonArrayVisualElement copy = Extensions.DefineElement(JsonVisualElementType.Array, parent) as JsonArrayVisualElement;
                
                foreach (var child in _childs)
                {
                    JsonFieldVisualElement childCopy = child.GetCopy(copy);
                    
                    //Debug.Log(copy.childCount);
                    
                    copy.SetValueElement(childCopy);
                    
                    //Debug.Log(copy.childCount);
                }
                
                // foreach (var visualElement in _root.Children())
                // {
                //     Debug.Log(_root.Q("Zalupa"));
                //     
                //     //var child = visualElement as JsonFieldVisualElement;
                //     //var copyChild = child.GetCopy();
                //     
                //     copy.SetValueElement(visualElement as JsonFieldVisualElement);
                // }
                
                return copy;
            }
        }
        
        private enum JsonVisualElementType
        {
            Value,
            Hashtable,
            Array
        }

        #endregion

        private static class Extensions
        {
            public static Dictionary<JsonVisualElementType, VisualTreeAsset> TypeMap = new()
            {
                { JsonVisualElementType.Value, AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathToValueVisualAsset) },
                { JsonVisualElementType.Array, AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathToArrayVisualAsset) },
                { JsonVisualElementType.Hashtable, AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathToHashtableVisualAsset) }
            };
            
            public static JsonFieldVisualElement DefineElement(JsonVisualElementType type, JsonFieldVisualElement parent)
            {
                switch (type)
                {
                    case JsonVisualElementType.Value:
                        return ResolveVisualElements<JsonValueVisualElement>(parent);
                    
                    case JsonVisualElementType.Hashtable:
                        return ResolveVisualElements<JsonHashtableVisualElement>(parent);
                    
                    case JsonVisualElementType.Array:
                        return ResolveVisualElements<JsonArrayVisualElement>(parent);
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            
            public static T ResolveVisualElements<T>(JsonFieldVisualElement first) where T: JsonFieldVisualElement
            {
                T second = GetVisualElement<T>();
                
                first.Add(second);

                second.style.paddingLeft = new StyleLength(first.style.paddingLeft.value.value + 15f);

                if (second is JsonHashtableVisualElement hashtableVisualElement)
                {
                    DropdownField dropdownField = hashtableVisualElement.Q<DropdownField>("ValueSelector");
                
                    dropdownField.RegisterCallback<ChangeEvent<string>>(evt =>
                    {
                        if (Enum.TryParse(evt.newValue, out JsonVisualElementType value))
                        {
                            hashtableVisualElement.SetValueElement(value);
                        }
                        else
                        {
                            if (evt.newValue == "Array/Value")
                                hashtableVisualElement.SetValueElement(JsonVisualElementType.Value);
                            else
                                hashtableVisualElement.SetValueElement(JsonVisualElementType.Hashtable);
                        }
                        
                        dropdownField.style.display = DisplayStyle.None;
                    });
                }
                else if (second is JsonArrayVisualElement arrayVisualElement)
                {
                    DropdownField dropdownField = arrayVisualElement.Q<DropdownField>("ValueSelector");
                
                    dropdownField.RegisterCallback<ChangeEvent<string>>(evt =>
                    {
                        if (Enum.TryParse(evt.newValue, out JsonVisualElementType value))
                        {
                            arrayVisualElement.SetValueElement(value);
                        }
                        else
                        {
                            if (evt.newValue == "Array/Value")
                                arrayVisualElement.SetValueElement(JsonVisualElementType.Value);
                            else
                                arrayVisualElement.SetValueElement(JsonVisualElementType.Hashtable);
                        }

                        dropdownField.style.display = DisplayStyle.None;
                    });
                }

                return second;
            }
            
            public static T GetVisualElement<T>() where T: JsonFieldVisualElement
            {
                if (typeof(JsonValueVisualElement) == typeof(T))
                    return new JsonValueVisualElement() as T;
            
                if (typeof(JsonArrayVisualElement) == typeof(T))
                    return new JsonArrayVisualElement() as T;
            
                return new JsonHashtableVisualElement() as T;
            }
        }
    }
}
#endif