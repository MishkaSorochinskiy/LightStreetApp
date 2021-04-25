using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class CameraInput
    {
        [Required]
        public string Photo { get; set; }

        [Required]
        public double? Latitude { get; set; }

        [Required]
        public double? Longtitude { get; set; }
    }
}
