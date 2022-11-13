using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightStreetUI.Data
{
    public class Camera
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
        public DateTime? LastAudit { get; set; }
        public string Identifier { get; set; }
    }
}
