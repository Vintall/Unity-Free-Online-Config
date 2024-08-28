using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity_Free_Online_Config.Editor.Enums;
using UnityEditor;
using UnityEngine;

namespace Unity_Free_Online_Config.Editor.Scripts
{
    public class CreateNewConfigView : EditorWindow
    {
        private string _newConfigName;
        private string _newConfigUrl;
        private string _targetPath = "Assets/Scripts/Configs/";
        private bool _embedNameIntoFile;
        private bool _embedUrlIntoFile;
        private Texture2D _backgroundTexture;
        private float baseTextureCoordsSize;
        private Rect _backgroundTextureCoords;
        private Vector2 _windowMinSizeVector;
        private Vector2 _windowMaxSizeVector;
        private ConfigVoFieldsScriptableObject _fieldsHolder;
        private SerializedObject _fieldsSerializedObject;
        private SerializedProperty _fieldsSerializedProperty;
        private GUILayoutOption[] _headerLabelOptions;
        private GUIStyle _headerLabelStyle;
        private GUILayoutOption[] _defaultLabelOptions;
        private GUIStyle _defaultLabelStyle;
        private GUILayoutOption[] _buttonOptions;
        private GUIStyle _buttonStyle;
        private const string MenuItem = "VUFOC/Create New Config";
        private const string WindowLabel = "Create new Spreadsheet config";
        private const string ConfigNameLabel = "Config Name";
        private const string SpreadsheetURLLabel = "Spreadsheet URL";
        private const string PathURLLabel = "Path for generated files";
        private const string DefaultPath = "Assets/VUFOC/<ConfigName>/";
        
        [MenuItem (MenuItem)]
        public static void  ShowWindow () => 
            GetWindow(typeof(CreateNewConfigView), true, "Create new config", true);

        private void InitializeGUIResources()
        {
            _headerLabelOptions = new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.Height(60)
            };
            
            _headerLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 30,
                alignment = TextAnchor.MiddleCenter
            };

            _defaultLabelOptions = new GUILayoutOption[]
            {
                GUILayout.Width(200),
                GUILayout.Height(20)
            };
            
            _defaultLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14
            };

            _buttonOptions = new GUILayoutOption[]
            {
                GUILayout.Width(170),
                GUILayout.Height(25)
            };
            
            _buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 16
            };
            
            _backgroundTexture = new Texture2D(32, 32);

            var textureSize = 32;
            var textureColor1 = new Color(0.22f, 0.22f, 0.22f, 0.25f);
            var textureColor2 = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            var textureLine1Width = 5;
            var textureLine2Width = 3;
            var textureLineFull = textureLine1Width + textureLine2Width;
            
            for (var x = 0; x < textureSize; ++x)
            for (var y = 0; y < textureSize; ++y)
            {
                if ((textureSize - x + y + textureLineFull - 1) % textureLineFull < textureLine1Width)
                    _backgroundTexture.SetPixel(x, y, textureColor1);
                else
                    _backgroundTexture.SetPixel(x, y, textureColor2);
            }
            
            _backgroundTexture.Apply();
            baseTextureCoordsSize = 10;
            _backgroundTextureCoords = new Rect(0, 0, baseTextureCoordsSize, baseTextureCoordsSize);
            _windowMinSizeVector = new Vector2(500, 300);
            _windowMaxSizeVector = new Vector2(500, 3000);
            
            minSize = _windowMinSizeVector;
            maxSize = _windowMaxSizeVector;
            
            _fieldsHolder = CreateInstance<ConfigVoFieldsScriptableObject>();
            _fieldsHolder.ConfigVoFields = new List<ConfigVoField>();
            _fieldsSerializedObject = new SerializedObject(_fieldsHolder);
            _fieldsSerializedProperty = _fieldsSerializedObject.FindProperty("ConfigVoFields");
        }
        
        private void OnGUI()
        {
            var ratio = rootVisualElement.contentRect.width / rootVisualElement.contentRect.height;
            var backgroundCoordHeight = baseTextureCoordsSize / ratio;
            
            if(_defaultLabelOptions == null || 
               _defaultLabelStyle == null ||
               _backgroundTexture == null ||
               _fieldsSerializedObject == null)
                InitializeGUIResources();

            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            
            // Background
            _backgroundTextureCoords = new Rect(0, -backgroundCoordHeight, baseTextureCoordsSize, backgroundCoordHeight);
            GUI.DrawTextureWithTexCoords(rootVisualElement.contentRect, _backgroundTexture, _backgroundTextureCoords);
            
            // Name
            //EditorGUILayout.LabelField(WindowLabel, _headerLabelStyle, _headerLabelOptions);
        
            // Path for generated configs
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(PathURLLabel, _defaultLabelStyle, _defaultLabelOptions);
            GUILayout.Box($"Using default path: {_targetPath}", GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            // Embed Name Toggle
            _embedNameIntoFile = EditorGUILayout.ToggleLeft("Embed name into file", _embedNameIntoFile, _defaultLabelStyle, _defaultLabelOptions);
            
            // Config Name input
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ConfigNameLabel, _defaultLabelStyle, _defaultLabelOptions);
            _newConfigName = EditorGUILayout.TextField(_newConfigName);
            EditorGUILayout.EndHorizontal();
            
            // Embed URL Toggle
            _embedUrlIntoFile = EditorGUILayout.ToggleLeft("Embed URL into file", _embedUrlIntoFile, _defaultLabelStyle, _defaultLabelOptions);

            
            if (_embedUrlIntoFile)
            {
                // Config Url input
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(SpreadsheetURLLabel, _defaultLabelStyle, _defaultLabelOptions);
                _newConfigUrl = EditorGUILayout.TextField(_newConfigUrl);
                EditorGUILayout.EndHorizontal();
            }
            
            // List of fields in VO
            EditorGUILayout.PropertyField(_fieldsSerializedProperty);
            
            //Create button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(1, true);
            
            if (GUILayout.Button(new GUIContent("Create config files"), _buttonStyle, _buttonOptions)) 
                OnCreateInput();
            
            EditorGUILayout.Space(1, true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10f);
            EditorGUILayout.EndHorizontal();
        }
        
        private void OnCreateInput()
        {
            if (_newConfigName == string.Empty)
                return;
            
            var fullPathConfig = _targetPath + $"{_newConfigName}/{_newConfigName}SpreadsheetConfig.cs";
            var fullPathVo = _targetPath + $"{_newConfigName}/{_newConfigName}SpreadsheetVo.cs";
            var propEnum = _fieldsSerializedProperty.GetEnumerator();
            var configVoFields = _fieldsSerializedProperty;
            var stringBuilder = new StringBuilder();
            propEnum.MoveNext();
            
            for (var i = 0; i < configVoFields.arraySize; ++i)
            {
                var voField = (ConfigVoField)((SerializedProperty)propEnum.Current)?.boxedValue;
                var fieldType = voField.FieldType;
                var fieldName = voField.FieldName;
                var publicPrefix = "public ";

                string type = fieldType switch
                {
                    EFieldType.String => "string",
                    EFieldType.Int => "int",
                    EFieldType.Float => "float",
                    EFieldType.Double => "double",
                    _ => ""
                };
            
                stringBuilder.Append("\t");
                stringBuilder.Append(publicPrefix);
                stringBuilder.Append(type);
                stringBuilder.Append(" ");
                stringBuilder.Append(fieldName);
                stringBuilder.Append(";\n");
                propEnum.MoveNext();
            }
            
            (propEnum as IDisposable).Dispose();
            
            Directory.CreateDirectory(_targetPath + _newConfigName);
            File.Create(fullPathConfig).Close();
            File.Create(fullPathVo).Close();
            
            File.WriteAllLines(fullPathVo, new string[]
            {
$@"using System;
using Models;
// Auto-generated file
[Serializable]
public class {_newConfigName}SpreadsheetVo : ASpreadsheetVo
{{
{stringBuilder}
}}"
            });
            
            var nameField = _embedUrlIntoFile
                ? ""
                : "\n\t[SerializeField] private string configName;";
            
            var nameProperty = _embedUrlIntoFile
                ? $"\tpublic override string ConfigName => \"{_newConfigName}\";"
                : "\tpublic override string ConfigName => configName;";

            var urlField = _embedUrlIntoFile
                ? ""
                : "\n\t[SerializeField] private string configDataUrl;";
            
            var urlProperty = _embedUrlIntoFile
                ? $"\tpublic override string ConfigDataUrl => \"{_newConfigUrl}\";"
                : "\tpublic override string ConfigDataUrl => configDataUrl;";

            File.WriteAllLines(fullPathConfig, new string[]
            {
$@"using UnityEngine;
using Models;
// Auto-generated file
[CreateAssetMenu(fileName = ""{_newConfigName}"", menuName = ""Configs/{_newConfigName}"")]
public class {_newConfigName}SpreadsheetConfig : ASpreadsheetConfig<{_newConfigName}SpreadsheetVo>
{{{nameField}{urlField}
    
{nameProperty}
{urlProperty}
    protected override ASpreadsheetVo TemplateVo => new {_newConfigName}SpreadsheetVo();
}}"
            });
            
            AssetDatabase.Refresh();
            
            Debug.Log(_targetPath + _newConfigName + "SpreadsheetConfig.cs");
        }
    }
}
