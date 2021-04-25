using BLL.Models;
using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CameraService
    {
        private UnitOfWork uof { get; }
        public CameraService(UnitOfWork uof)
        {
            this.uof = uof;
        }

        public IQueryable<Camera> GetAll()
        {
            return uof.Repository<Camera>().Get();
        }

        public async Task<Camera> CreateAsync(CameraInput cameraInput)
        {
            Camera camera = new Camera
            {
                Photo = cameraInput.Photo,
                Latitude = cameraInput.Latitude,
                Longtitude = cameraInput.Longtitude,
                CreateTime = DateTime.UtcNow
            };

            uof.Repository<Camera>().Insert(camera);

            await uof.SaveChangesAsync();

            return camera;
        }
    }
}
