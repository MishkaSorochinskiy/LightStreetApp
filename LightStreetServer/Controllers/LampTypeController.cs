using BLL.Services;
using DAL;
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

        private UnitOfWork uof;

        public LampTypeController(LampTypeService lampTypeService, UnitOfWork unitOfWork)
        {
            LampTypeService = lampTypeService;

            uof = unitOfWork;
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

        [HttpPost]
        public async Task<LampType> CreateLampType(LampType lampType)
        {
            var newType = uof.Repository<LampType>().Insert(lampType);

            await uof.SaveChangesAsync();

            return newType;
        }
    }
}
