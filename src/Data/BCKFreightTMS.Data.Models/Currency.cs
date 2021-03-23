namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Currency : SettingModel
    {
        public Currency()
        {
            this.Companies = new HashSet<Company>();
            this.OrderTosIn = new HashSet<OrderTo>();
            this.OrderTosOut = new HashSet<OrderTo>();
        }

        [Required]
        public override string Name { get; set; }

        public decimal Rate { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        [InverseProperty("CurrencyIn")]
        public virtual ICollection<OrderTo> OrderTosIn { get; set; }

        [InverseProperty("CurrencyOut")]
        public virtual ICollection<OrderTo> OrderTosOut { get; set; }
    }
}
