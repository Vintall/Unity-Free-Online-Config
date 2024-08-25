using System.Collections.Generic;
using Extensions;
using UnityEngine;

public abstract class ALoadableVufocDatabase : ScriptableObject
{
    public abstract string DatabaseName { get; }
    public abstract string DatabaseDataUrl { get; }
    //public abstract string DatabaseFallbackUrl { get; }

    public void LoadSpreadsheetDataAsync() => 
        VufocExtensions.LoadSpreadsheetAsync(DatabaseDataUrl, OnDatabaseDataLoaded);
    
    public string LoadSpreadsheetData() => 
        VufocExtensions.LoadSpreadsheet(DatabaseDataUrl);
    
    //public void LoadSpreadsheetFallback() => VufocExtensions.LoadSpreadsheetAsync(DatabaseFallbackUrl, OnDatabaseFallbackLoaded);

    protected abstract void OnDatabaseDataLoaded(string loadedData);
    //protected abstract void OnDatabaseFallbackLoaded(string loadedData);
}