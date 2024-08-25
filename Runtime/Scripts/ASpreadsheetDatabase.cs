using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

public abstract class ASpreadsheetDatabase<T> : ALoadableVufocDatabase where T : ASpreadsheetVo
{
    [SerializeField] protected List<SpreadsheetVo<T>> spreadsheetEntries;

    protected abstract ASpreadsheetVo TemplateVo { get; }

    protected override void OnDatabaseDataLoaded(string loadedData)
    {
        var loadedLines = loadedData.Split('\n');
        spreadsheetEntries = new List<SpreadsheetVo<T>>(loadedLines.Length);

        for (var lineI = 0; lineI < loadedLines.Length; ++lineI)
        {
            var line = loadedLines[lineI].Split("\t");
            var spreadsheetVo = new SpreadsheetVo<T>();
            var spreadsheetDataVo = TemplateVo;
            
            spreadsheetVo.InitializeData(spreadsheetDataVo);
            FillEntry(line, (T)spreadsheetDataVo);
            spreadsheetEntries.Add(spreadsheetVo);
        }
    }

    // protected override void OnDatabaseFallbackLoaded(string loadedData)
    // {
    //     var loadedLines = loadedData.Split('\n');
    //     spreadsheetEntries = new List<SpreadsheetVo<T>>(loadedLines.Length);
    //
    //     for (var lineI = 0; lineI < loadedLines.Length; ++lineI)
    //     {
    //         var line = loadedLines[lineI].Split("\t");
    //         var spreadsheetDataVo = TemplateVo;
    //         
    //         FillEntry(line, (T)spreadsheetDataVo);
    //     }
    // }

    private void FillEntry(string[] dataLine, T target)
    {
        var fieldInfos = typeof(T).GetFields();
        
        for (var fieldI = 0; fieldI < fieldInfos.Length; ++fieldI)
        {
            var fieldData = dataLine[fieldI];
            var fieldInfo = fieldInfos[fieldI];
            var fieldType = fieldInfo.FieldType;
                
            switch (fieldType.Name)
            {
                case "String":
                    fieldInfo.SetValue(target, fieldData);
                    break;
                case "Int32":
                    fieldInfo.SetValue(target, int.Parse(fieldData));
                    break;
                case "Single":
                    fieldInfo.SetValue(target, float.Parse(fieldData.Replace(',', '.')));
                    break;
                case "Double":
                    fieldInfo.SetValue(target, double.Parse(fieldData.Replace(',', '.')));
                    break;
            }
        }
    }
}
