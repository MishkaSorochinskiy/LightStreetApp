using BLL.InformationalServices;
using BLL.Models;
using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LampService
    {
        private UnitOfWork uof { get; }
        public ImageAnalyser ImageAnalyser { get; }

        public LampService(UnitOfWork uof, ImageAnalyser imageAnalyser)
        {
            this.uof = uof;

            ImageAnalyser = imageAnalyser;
        }

        public IQueryable<Lamp> GetAll()
        {
            return uof.Repository<Lamp>().Get();
        }

        public async Task<List<LampLightOutput>> GetLampLightsAsync(List<int> cameraIds)
        {
            var cameras = uof.Repository<Camera>().Get(cmr => cameraIds.Contains(cmr.Id)).ToList();

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

            return new LampLightOutput
            {
                CameraId = camera.Id,
                IsLight = lightnessResponce.Lightness > 30
            };
        }

        #endregion
    }
}
