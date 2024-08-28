using System;
using System.Collections.Generic;
using Models;
using Unity_Free_Online_Config.Runtime.Scripts;
using UnityEngine;

public abstract class ASpreadsheetConfig<T> : ALoadableConfig where T : ASpreadsheetVo
{
    [SerializeField] protected List<SpreadsheetConfigVo<T>> spreadsheetEntries;

    protected abstract ASpreadsheetVo TemplateVo { get; }

    protected override void OnConfigDataLoaded(string loadedData)
    {
        var loadedLines = loadedData.Split('\n');
        spreadsheetEntries = new List<SpreadsheetConfigVo<T>>(loadedLines.Length);

        for (var lineI = 0; lineI < loadedLines.Length; ++lineI)
        {
            var line = loadedLines[lineI].Split("\t");
            var spreadsheetVo = new SpreadsheetConfigVo<T>();
            var spreadsheetDataVo = TemplateVo;
            
            spreadsheetVo.InitializeData(spreadsheetDataVo);
            FillEntry(line, (T)spreadsheetDataVo);
            spreadsheetEntries.Add(spreadsheetVo);
        }
    }

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
