using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Unity_Free_Online_Config.Editor.Scripts
{

    public class CreateNewConfigView : EditorWindow
    {
        private string _newConfigName;
        private string _newConfigUrl;
        private string _targetPath = "Assets/VUFOC/";
        private bool _embedNameIntoFile;
        private bool _embedUrlIntoFile;
        private Texture2D _backgroundTexture;
        private Rect _backgroundTextureCoords;
        private Vector2 _windowMinSizeVector;
        private Vector2 _windowMaxSizeVector;
        private const string MenuItem = "VOUFO/Create New Config";
        private const string WindowLabel = "Create new Spreadsheet config";
        private const string ConfigNameLabel = "Config Name";
        private const string SpreadsheetURLLabel = "Spreadsheet URL";
        private const string PathURLLabel = "Path for generated files";
        private const string DefaultPath = "Assets/VUFOC/<ConfigName>/";
        
        private GUILayoutOption[] _headerLabelOptions;
        private GUIStyle _headerLabelStyle;
        private GUILayoutOption[] _defaultLabelOptions;
        private GUIStyle _defaultLabelStyle;
        private GUILayoutOption[] _buttonOptions;
        private GUIStyle _buttonStyle;
        
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
                //if(7 + (i - j) % 8 < 5)
                //    _backgroundTexture.SetPixel(i, j, new Color(0.21f, 0.225f, 0.225f, 1));
                if ((textureSize - x + y + textureLineFull - 1) % textureLineFull < textureLine1Width)
                    _backgroundTexture.SetPixel(x, y, textureColor1);
                else
                    _backgroundTexture.SetPixel(x, y, textureColor2);
            }
            
            _backgroundTexture.Apply();
            _backgroundTextureCoords = new Rect(0, 0, 10, 10);
            _windowMinSizeVector = new Vector2(500, 500);
            _windowMaxSizeVector = new Vector2(500, 500);
            
            minSize = _windowMinSizeVector;
            maxSize = _windowMaxSizeVector;
        }

        private void OnGUI()
        {
            if(_defaultLabelOptions == null || 
               _defaultLabelStyle == null ||
               _backgroundTexture == null)
                InitializeGUIResources();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            
            // Background
            GUI.DrawTextureWithTexCoords(rootVisualElement.contentRect, _backgroundTexture, _backgroundTextureCoords);
            
            // Name
            EditorGUILayout.LabelField(WindowLabel, _headerLabelStyle, _headerLabelOptions);
        
            // Config Name input
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ConfigNameLabel, _defaultLabelStyle, _defaultLabelOptions);
            _newConfigName = EditorGUILayout.TextField(_newConfigName);
            EditorGUILayout.EndHorizontal();
        
            // Config Url input
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(SpreadsheetURLLabel, _defaultLabelStyle, _defaultLabelOptions);
            _newConfigUrl = EditorGUILayout.TextField(_newConfigUrl);
            EditorGUILayout.EndHorizontal();
            
            // Path for generated databases
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(PathURLLabel, _defaultLabelStyle, _defaultLabelOptions);
            //_targetPath = EditorGUILayout.TextField(_targetPath);
            GUILayout.Box($"Using default path: {_targetPath}", GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            _embedNameIntoFile = EditorGUILayout.ToggleLeft("Embed name into file", _embedNameIntoFile, _defaultLabelStyle, _defaultLabelOptions);
            _embedUrlIntoFile = EditorGUILayout.ToggleLeft("Embed URL into file", _embedUrlIntoFile, _defaultLabelStyle, _defaultLabelOptions);
            
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
            var fullPathConfig = _targetPath + $"{_newConfigName}/{_newConfigName}SpreadsheetConfig.cs";
            var fullPathVo = _targetPath + $"{_newConfigName}/{_newConfigName}SpreadsheetVo.cs";
            
            Directory.CreateDirectory(_targetPath + _newConfigName);
            File.Create(fullPathConfig).Close();
            File.Create(fullPathVo).Close();
            
            File.WriteAllLines(fullPathVo, new string[]
            {
$@"
using System;
using Models;
// Auto-generated file
[Serializable]
public class {_newConfigName}SpreadsheetVo : ASpreadsheetVo
{{
    public string ExampleString;
    public int ExampleInt;
    public float ExampleFloat;
    public double ExampleDouble;
}}
"
            });
            
            File.WriteAllLines(fullPathConfig, new string[]
            {
$@"
using UnityEngine;
using Models;
// Auto-generated file
[CreateAssetMenu(fileName = ""{_newConfigName}"", menuName = ""Databases/{_newConfigName}"")]
public class {_newConfigName}SpreadsheetDatabase : ASpreadsheetDatabase<{_newConfigName}SpreadsheetVo>
{{
    [SerializeField] private string databaseName;
    [SerializeField] private string databaseDataUrl;
    public override string DatabaseName => databaseName;
    public override string DatabaseDataUrl => databaseDataUrl;
    protected override ASpreadsheetVo TemplateVo => new {_newConfigName}SpreadsheetVo();
}}
"
            });
            
            AssetDatabase.Refresh();
            
            Debug.Log(_targetPath + _newConfigName + "SpreadsheetConfig.cs");
        }
    }
}
