using BLL.InformationalServices;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LightStreetServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LampController : ControllerBase
    {
        public ImageAnalyser ImageAnalyser { get; }
        public CameraService CameraService { get; }

        public LampController(ImageAnalyser imageAnayser, CameraService cameraService)
        {
            ImageAnalyser = imageAnayser;

            CameraService = cameraService;
        }

        [HttpPost("analyse")]
        public LampInfoOutput Analyse(LampInfoInput model)
        {
            return LampAnalyser.Predict(model);
        }

        [HttpPost("analyse-multiple")]
        public async Task<List<LampLightOutput>> AnalyseMultiple(List<int> cameraIds)
        {
            var responce = await CameraService.GetLampLightsAsync(cameraIds);

            return responce;
        }

        [HttpPost("detect")]
        public async Task<DetectOutput> Detect(IFormFile file)
        {
            byte[] imgArr;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                 
                imgArr = memoryStream.ToArray();
            }

            var predictionResponce = await ImageAnalyser.DetectAsync(imgArr);

            var detectResponce =  ImageAnalyser.GetLightness(imgArr, predictionResponce);

            return detectResponce;
        }
    }
}
