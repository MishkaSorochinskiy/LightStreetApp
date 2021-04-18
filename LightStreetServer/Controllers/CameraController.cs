using BLL.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightStreetServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CameraController : ControllerBase
    {
        public CameraService CameraService { get;}
        public CameraController(CameraService cameraService)
        {
            CameraService = cameraService;
        }

        [HttpGet]
        public IQueryable<Camera> Get()
        {
            return CameraService.GetAll();
        }

        [HttpGet("{id}")]
        public Camera GetById(int id)
        {
            return CameraService.GetAll().Where(entity => entity.Id == id).FirstOrDefault();
        }
    }
}
