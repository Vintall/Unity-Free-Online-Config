using System;
using Models;

namespace Samples.Scripts
{
    [Serializable]
    public class ExampleSpreadsheetVo : ASpreadsheetDataVo
    {
        public string String;
        public int Int;
        public float Float;
        public double Double;
    }
}