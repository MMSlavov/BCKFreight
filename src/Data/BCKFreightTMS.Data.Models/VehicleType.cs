﻿namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class VehicleType : BaseDeletableModel<int>
    {
        public VehicleType()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string MyProperty { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
