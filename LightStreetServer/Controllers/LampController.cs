using BLL.InformationalServices;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpPost("detect")]
        public async Task<DetectOutput> Detect(IFormFile file)
        {
            var detectResponce = await ImageAnalyser.DetectAsync(file);

            return detectResponce;
        }
    }
}
