namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderTo : BaseDeletableModel<int>
    {
        public OrderTo()
        {
            this.Drivers = new HashSet<DriverOrder>();
        }

        public string ReferenceNum { get; set; }

        public decimal PriceNetOut { get; set; }

        public int DueDays { get; set; }

        [Required]
        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Vehicle))]
        public string VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public OrderType Type { get; set; }

        public string ContactId { get; set; }

        public Person Contact { get; set; }

        public Order Order { get; set; }

        public virtual ICollection<DriverOrder> Drivers { get; set; }
    }
}
