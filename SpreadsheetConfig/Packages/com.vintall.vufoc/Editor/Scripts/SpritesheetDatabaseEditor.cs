using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [CustomEditor(typeof(ALoadableVufocDatabase), true)]
    public class SpreadsheetDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Download Data"))
            {
                ((ALoadableVufocDatabase)target).LoadSpreadsheetDataAsync();
            }

            //if (GUILayout.Button("Download Fallback"))
            //{
            //    ((ALoadableVufocDatabase)target).LoadSpreadsheetFallback();
            //}
            
            GUILayout.EndHorizontal();
        
            base.OnInspectorGUI();
        }
    }
}