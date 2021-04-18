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
    public class LampTypeController : ControllerBase
    {
        public LampTypeService LampTypeService { get; }
        public LampTypeController(LampTypeService lampTypeService)
        {
            LampTypeService = lampTypeService;
        }

        [HttpGet]
        public IQueryable<LampType> Get()
        {
            return LampTypeService.GetAll();
        }

        [HttpGet("{id}")]
        public LampType GetById(int id)
        {
            return LampTypeService.GetAll().Where(entity => entity.Id == id).FirstOrDefault();
        }
    }
}
