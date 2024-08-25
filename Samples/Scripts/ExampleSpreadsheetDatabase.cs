using Models;
using UnityEngine;

namespace Samples.Scripts
{
    [CreateAssetMenu(fileName = "Example Spreadsheet Database", menuName = "Databases/ExampleSpreadsheetDatabase")]
    public class ExampleSpreadsheetDatabase : AVufocDatabase<ExampleSpreadsheetVo>
    {
        [SerializeField] private string databaseName;
        [SerializeField] private string databaseDataUrl;
        //[SerializeField] private string databaseFallbackUrl;
        
        public override string DatabaseName => databaseName;
        public override string DatabaseDataUrl => databaseDataUrl;
        //public override string DatabaseFallbackUrl => databaseFallbackUrl;
        protected override ASpreadsheetDataVo TemplateVo => new ExampleSpreadsheetVo();
    }
}