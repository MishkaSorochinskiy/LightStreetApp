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
    public class LampController : ControllerBase
    {
        public LampService LampService { get; }
        public LampController(LampService lampService)
        {
            LampService = lampService;
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
    }
}
