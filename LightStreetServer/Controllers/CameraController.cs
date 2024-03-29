﻿using BLL.InformationalServices;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using Microsoft.AspNet.OData;
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
        [EnableQuery]
        public IQueryable<Camera> Get()
        {
            return CameraService.GetAll();
        }

        [HttpGet("{id}")]
        public Camera GetById(int id)
        {
            return CameraService.GetAll().Where(entity => entity.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public async Task<Camera> PostCamera(CameraInput camera)
        {
            var responce = await CameraService.CreateAsync(camera);
            
            return responce;
        }

        [HttpPost("audit/{id}")]
        public async Task<IActionResult> AuditCamera(int id)
        {
            await CameraService.AuditAsync(id);

            return Ok();
        }
    }
}
