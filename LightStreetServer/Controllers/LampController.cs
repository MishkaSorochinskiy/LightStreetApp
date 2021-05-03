using BLL.InformationalServices;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LightStreetServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LampController : ControllerBase
    {
        public LampService LampService { get; }
        public ImageAnalyser ImageAnalyser { get; }

        public LampController(LampService lampService, ImageAnalyser imageAnayser)
        {
            LampService = lampService;
            ImageAnalyser = imageAnayser;
        }

        [HttpGet]
        public IQueryable<Lamp> Get()
        {
            return LampService.GetAll();
        }

        [HttpGet("{id}")]
        public Lamp GetById(int id)
        {
            return LampService.GetAll().Where(entity => entity.Id == id).FirstOrDefault();
        }

        [HttpPost("analyse")]
        public LampInfoOutput Analyse(LampInfoInput model)
        {
            return LampAnalyser.Predict(model);
        }

        [HttpPost("analyse-multiple")]
        public async Task<List<LampLightOutput>> AnalyseMultiple(List<int> cameraIds)
        {
            var responce = await LampService.GetLampLightsAsync(cameraIds);

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
