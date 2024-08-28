using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class SpreadsheetConfigVo<T> where T : ASpreadsheetVo
    {
        [SerializeField] private T configData;
        //[SerializeField] private T configFallback;

        public T ConfigData => configData;
        //public T ConfigFallback => configFallback;

        public void InitializeData(ASpreadsheetVo config/*, ASpreadsheetDataVo configFallback*/)
        {
            this.configData = (T)config;
            //this.configFallback = (T)configFallback;
        }
    }
}