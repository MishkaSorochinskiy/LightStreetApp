using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DAL.Entities
{
    public partial class LampType:IEntity
    {
        public LampType()
        {
            Lamps = new HashSet<Lamp>();
        }
        [Key]
        [Column("LampTypeId")]
        public int Id { get; set; }
        public double? Power { get; set; }
        public double? Distance { get; set; }
        public int? Material { get; set; }
        public int? Type { get; set; }

        public virtual ICollection<Lamp> Lamps { get; set; }
    }
}
