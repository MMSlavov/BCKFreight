namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class CargoType : SettingModel
    {
        public CargoType()
        {
            this.Cargos = new HashSet<Cargo>();
        }

        [Required]
        [MaxLength(50)]
        public override string Name { get; set; }

        public virtual ICollection<Cargo> Cargos { get; set; }
    }
}
