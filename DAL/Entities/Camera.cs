using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DAL.Entities
{
    public partial class Camera:IEntity
    {
        public Camera()
        {
        }

        [Key]
        [Column("CameraId")]
        public int Id { get; set; }
        public string Photo { get; set; }
        public int? LampTypeId { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual LampType LampType { get; set; }
    }
}
