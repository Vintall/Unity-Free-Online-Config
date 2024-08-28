using Unity_Free_Online_Config.Runtime.Scripts.Extensions;
using UnityEngine;

namespace Unity_Free_Online_Config.Runtime.Scripts
{
    public abstract class ALoadableConfig : ScriptableObject
    {
        public abstract string ConfigName { get; }
        public abstract string ConfigDataUrl { get; }

        public void LoadSpreadsheetDataAsync() => 
            SpreadsheetExtensions.LoadSpreadsheetAsync(ConfigDataUrl, OnConfigDataLoaded);
    
        public string LoadSpreadsheetData() => 
            SpreadsheetExtensions.LoadSpreadsheet(ConfigDataUrl);
    
        protected abstract void OnConfigDataLoaded(string loadedData);
    }
}