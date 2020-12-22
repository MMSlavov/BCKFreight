namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class VehicleLoadingBody : SettingModel
    {
        public VehicleLoadingBody()
        {
            this.Vehicles = new HashSet<Vehicle>();
            this.Cargos = new HashSet<Cargo>();
        }

        [Required]
        [MaxLength(80)]
        public override string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public virtual ICollection<Cargo> Cargos { get; set; }
    }
}
