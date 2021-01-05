namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class Currency : SettingModel
    {
        public Currency()
        {
            this.Companies = new HashSet<Company>();

            this.OrderTos = new HashSet<OrderTo>();

            this.OrderFroms = new HashSet<OrderFrom>();
        }

        [Required]
        public override string Name { get; set; }

        public decimal Rate { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }

        public virtual ICollection<OrderFrom> OrderFroms { get; set; }
    }
}
