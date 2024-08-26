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

        private void Awake()
        {
            InitializeGUIResources();
        }

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
                GUILayout.Width(100),
                GUILayout.Height(20)
            };
            
            _buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 16
            };
            
            _backgroundTexture = new Texture2D(32, 32);

            for (var i = 0; i < 32; ++i)
            for (var j = 0; j < 32; ++j)
            {
                if ((i + j) % 12 <= 6)
                    _backgroundTexture.SetPixel(i, j, new Color(0.25f, 0.25f, 0.25f, 1));
                else
                    _backgroundTexture.SetPixel(i, j, new Color(0.2f, 0.2f, 0.2f, 1));
            }
            
            _backgroundTexture.Apply();
            _backgroundTextureCoords = new Rect(0, 0, 40, 40);
            _windowMinSizeVector = new Vector2(500, 220);
            _windowMaxSizeVector = new Vector2(500, 500);
        }

        private void OnGUI()
        {
            if(_defaultLabelOptions == null || 
               _defaultLabelStyle == null)
                InitializeGUIResources();
            
            minSize = _windowMinSizeVector;
            maxSize = _windowMaxSizeVector;
            
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
            GUILayout.Box($"Using default path: {_targetPath} for now.");
            EditorGUILayout.EndHorizontal();

            _embedNameIntoFile = EditorGUILayout.ToggleLeft("Embed name into file", _embedNameIntoFile, _defaultLabelStyle, _defaultLabelOptions);
            _embedUrlIntoFile = EditorGUILayout.ToggleLeft("Embed URL into file", _embedUrlIntoFile, _defaultLabelStyle, _defaultLabelOptions);
            
            //Create button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(1, true);
            
            if (GUILayout.Button(new GUIContent("Create"), _buttonStyle, _buttonOptions)) 
                OnCreateInput();
            
            EditorGUILayout.Space(1, true);
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
