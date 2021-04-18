using BLL.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLL.InformationalServices
{
    public class LampAnalyser
    {
        private static Lazy<PredictionEngine<LampInfoInput, LampInfoOutput>> PredictionEngine = new Lazy<PredictionEngine<LampInfoInput, LampInfoOutput>>(CreatePredictionEngine);

        public static LampInfoOutput Predict(LampInfoInput input)
        {
            LampInfoOutput result = PredictionEngine.Value.Predict(input);
            return result;
        }

        public static PredictionEngine<LampInfoInput, LampInfoOutput> CreatePredictionEngine()
        {
            MLContext mlContext = new MLContext();

            string modelPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, @"BLL\InformationalServices\MLModel.zip");
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<LampInfoInput, LampInfoOutput>(mlModel);

            return predEngine;
        }
    }
}
