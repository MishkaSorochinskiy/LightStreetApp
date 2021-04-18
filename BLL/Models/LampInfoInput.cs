using Microsoft.ML.Data;

namespace BLL.Models
{
    public class LampInfoInput
    {
        [ColumnName("Lightness"), LoadColumn(0)]
        public string Lightness { get; set; }


        [ColumnName("Power"), LoadColumn(1)]
        public float Power { get; set; }


        [ColumnName("Distance"), LoadColumn(2)]
        public string Distance { get; set; }


        [ColumnName("Type"), LoadColumn(3)]
        public float Type { get; set; }


        [ColumnName("Material"), LoadColumn(4)]
        public float Material { get; set; }


        [ColumnName("Result"), LoadColumn(5)]
        public string Result { get; set; }
    }
}
