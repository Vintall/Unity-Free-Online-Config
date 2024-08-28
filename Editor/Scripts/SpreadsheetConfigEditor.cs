using Unity_Free_Online_Config.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace Unity_Free_Online_Config.Editor.Scripts
{
    [CustomEditor(typeof(ALoadableConfig), true)]
    public class SpreadsheetConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Download Data")) 
                ((ALoadableConfig)target).LoadSpreadsheetDataAsync();
            
            GUILayout.EndHorizontal();
        
            base.OnInspectorGUI();
        }
    }
}