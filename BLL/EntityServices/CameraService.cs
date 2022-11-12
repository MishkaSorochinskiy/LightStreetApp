using BLL.InformationalServices;
using BLL.Models;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CameraService
    {
        public ImageAnalyser ImageAnalyser { get; }
        private UnitOfWork uof { get; }
        public CameraService(UnitOfWork uof, ImageAnalyser imageAnalasyer)
        {
            ImageAnalyser = imageAnalasyer;
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
                LampTypeId = cameraInput.LampTypeId,
                LastAudit = DateTime.Now,
                Identifier = Guid.NewGuid().ToString(),
                CreateTime = DateTime.UtcNow
            };

            uof.Repository<Camera>().Insert(camera);

            await uof.SaveChangesAsync();

            return camera;
        }

        public async Task<List<LampLightOutput>> GetLampLightsAsync(List<int> cameraIds)
        {
            var cameras = uof.Repository<Camera>().Get(cmr => cameraIds.Contains(cmr.Id), includeProperties: "LampType").ToList();

            List<Task<LampLightOutput>> cameraLightTasks = new List<Task<LampLightOutput>>();

            foreach (var camera in cameras)
                cameraLightTasks.Add(Task.Run(() => ProcessLampLightsAsync(camera)));

            await Task.WhenAll(cameraLightTasks);

            var lightsResponce = new List<LampLightOutput>();

            cameraLightTasks.ForEach(lightTask => lightsResponce.Add(lightTask.Result));

            return lightsResponce;
        }

        #region private methods

        private async Task<LampLightOutput> ProcessLampLightsAsync(Camera camera)
        {
            var bytes = Convert.FromBase64String(camera.Photo.Remove(0, camera.Photo.IndexOf(',') + 1));

            var detectResponce = await ImageAnalyser.DetectAsync(bytes);

            var lightnessResponce = ImageAnalyser.GetLightness(bytes, detectResponce, false);

            LampInfoInput lampInfoInput = new LampInfoInput
            {
                Lightness = lightnessResponce.Lightness.ToString(),
                Power = (float)camera.LampType.Power,
                Distance = camera.LampType.Distance.ToString(),
                Type = (float)camera.LampType.Type,
                Material = (float)camera.LampType.Material
            };

            return new LampLightOutput
            {
                CameraId = camera.Id,
                IsLight = LampAnalyser.Predict(lampInfoInput).Prediction == "1"
            };
        }

        #endregion
    }
}
