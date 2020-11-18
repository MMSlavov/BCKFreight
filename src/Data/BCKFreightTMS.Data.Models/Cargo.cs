namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Cargo : BaseDeletableModel<string>
    {
        public Cargo()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<Order>();
        }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public CargoType Type { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [Column(TypeName = "real")]
        public double Lenght { get; set; }

        [Column(TypeName = "real")]
        public double Width { get; set; }

        [Column(TypeName = "real")]
        public double Height { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal WeightGross { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal WeightNet { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cubature { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "text")]
        public string MyProperty { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
