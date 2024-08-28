using Models;
using Samples.Scripts;
using UnityEngine;

namespace Unity_Free_Online_Config.Samples.Scripts
{
    [CreateAssetMenu(fileName = "Example Spreadsheet Database", menuName = "Databases/ExampleSpreadsheetDatabase")]
    public class ExampleSpreadsheetDatabase : ASpreadsheetDatabase<ExampleSpreadsheetVo>
    {
        [SerializeField] private string databaseName;
        [SerializeField] private string databaseDataUrl;
        
        public override string DatabaseName => databaseName;
        public override string DatabaseDataUrl => databaseDataUrl;
        protected override ASpreadsheetVo TemplateVo => new ExampleSpreadsheetVo();
    }
}