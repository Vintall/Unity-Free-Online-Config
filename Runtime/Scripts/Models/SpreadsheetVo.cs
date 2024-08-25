using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class SpreadsheetVo<T> where T : ASpreadsheetDataVo
    {
        [SerializeField] private T configData;
        //[SerializeField] private T configFallback;

        public T ConfigData => configData;
        //public T ConfigFallback => configFallback;

        public void InitializeData(ASpreadsheetDataVo configData/*, ASpreadsheetDataVo configFallback*/)
        {
            this.configData = (T)configData;
            //this.configFallback = (T)configFallback;
        }
    }
}