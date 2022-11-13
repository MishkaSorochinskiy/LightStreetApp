using Microsoft.ML.Data;
using System;

namespace BLL.Models
{
    public class LampInfoOutput
    {
        [ColumnName("PredictedLabel")]
        public String Prediction { get; set; }
        public float[] Score { get; set; }
    }
}
