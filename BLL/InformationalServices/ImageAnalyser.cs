using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLL.InformationalServices
{
    public class ImageAnalyser
    {
        private readonly IConfiguration _config;

        public ImageAnalyser(IConfiguration config)
        {
            this._config = config;
        }

        public async Task<DetectOutput> DetectAsync(IFormFile file)
        {
            HttpClient client = new HttpClient();
            var predictionSection = _config.GetSection("PredictionApi");

            var request = new HttpRequestMessage(HttpMethod.Post, predictionSection["url"]);
            request.Headers.Add("Prediction-Key", predictionSection["key"]);

            var stream = file.OpenReadStream();
            request.Content = new StreamContent(stream);
            var responce = await client.SendAsync(request);      
            string jsonResponce = await responce.Content.ReadAsStringAsync();   
            ImagePrediction predictionResult = JsonConvert.DeserializeObject<ImagePrediction>(jsonResponce);

            return DrawRectangles(file, predictionResult);
        }
        private DetectOutput DrawRectangles(IFormFile file, ImagePrediction predictionResult)
        {
            SKBitmap bitmap = SKBitmap.Decode(file.OpenReadStream());
            SKCanvas canvas = new SKCanvas(bitmap);
            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                Color = new SKColor(220, 38, 38),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 3
            };

            double lumSum = 0;
            int lumCount = 0;
            foreach (var prediction in predictionResult.Predictions)
            {
                if (prediction.Probability > 0.6)
                {
                    var x = prediction.BoundingBox.Left * bitmap.Width;
                    var y = prediction.BoundingBox.Top * bitmap.Height;

                    var width = prediction.BoundingBox.Width * bitmap.Width;
                    var height = prediction.BoundingBox.Height * bitmap.Height;

                    for (int i = (int)Math.Floor(x); i < (int)Math.Ceiling(x+width+(width*0.15)); ++i)
                    {
                        for(int j = (int)Math.Floor(y); j< (int)Math.Ceiling(y+height+(height*0.3)); ++j)
                        {
                            var pixel = bitmap.GetPixel(i, j);

                            var rlin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(pixel.Red));
                            var glin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(pixel.Green));
                            var blin = PixelInfo.ConvertToLinear(PixelInfo.ConvertToDecimal(pixel.Blue));

                            var Y = PixelInfo.CalculateYLuminence(rlin, glin, blin);

                            var lum = PixelInfo.CalculatePerceivedLightness(Y);
                            if (lum > PixelInfo.LIGHTNESS)
                            {
                                lumSum += lum;
                                lumCount += 1;

                                var green = (byte)(255 * (lum / 100));
                                if (green < 125)
                                    green = 140;

                                bitmap.SetPixel(i, j, new SKColor(103, (byte)green, 52));
                            }
                        }
                    }
                }
            }

            var image = SKImage.FromBitmap(bitmap);

            var imageStream = image.Encode().AsStream();

            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);

                var bytesArray = ms.ToArray();

                return new DetectOutput
                {
                    Image64 = Convert.ToBase64String(bytesArray),
                    Lightness = Math.Floor(lumSum / lumCount)
                };
            }
        }
    }
}
