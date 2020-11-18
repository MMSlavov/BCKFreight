namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Vehicle : BaseDeletableModel<string>
    {
        public Vehicle()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<OrderTo>();
        }

        [Required]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public VehicleType Type { get; set; }

        [ForeignKey(nameof(LoadingBody))]
        public int LoadingBodyId { get; set; }

        public VehicleLoadingBody LoadingBody { get; set; }

        [ForeignKey(nameof(Driver))]
        public string DriverId { get; set; }

        public Person Driver { get; set; }

        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Trailer))]
        public string TrailerId { get; set; }

        public Vehicle Trailer { get; set; }

        [Required]
        [MaxLength(10)]
        public string RegNumber { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Details { get; set; }

        public virtual ICollection<OrderTo> Orders { get; set; }
    }
}
