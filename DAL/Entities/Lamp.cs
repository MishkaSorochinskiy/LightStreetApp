using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DAL.Entities
{
    public partial class Lamp:IEntity
    {
        [Key]
        [Column("LampId")]
        public int Id { get; set; }
        public int? LampTypeId { get; set; }
        public int? CameraId { get; set; }
        public double? TopCoordinate { get; set; }
        public double? LeftCoordinate { get; set; }
        public DateTime? CreateTime { get; set; }

        public virtual Camera Camera { get; set; }
        public virtual LampType LampType { get; set; }
    }
}
