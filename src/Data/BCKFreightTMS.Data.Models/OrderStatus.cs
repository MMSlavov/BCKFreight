namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderStatus : BaseDeletableModel<int>
    {
        public OrderStatus()
        {
            this.Orders = new HashSet<Order>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
