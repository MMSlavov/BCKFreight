namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderType : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<OrderTo> OrdersTo { get; set; }

        public virtual ICollection<OrderFrom> OrdersFrom { get; set; }
    }
}
