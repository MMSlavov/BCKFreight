namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class CarrierOrder : BaseDeletableModel<string>
    {
        public CarrierOrder()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderTos = new HashSet<OrderTo>();
        }

        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        [MaxLength(15)]
        public string ReferenceNum { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }
    }
}
