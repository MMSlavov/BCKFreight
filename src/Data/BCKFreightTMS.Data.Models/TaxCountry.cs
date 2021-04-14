namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class TaxCountry : BaseDeletableModel<int>
    {
        public TaxCountry()
        {
            this.Companies = new HashSet<Company>();
            this.OrderActions = new HashSet<OrderAction>();
        }

        [Required]
        [MaxLength(90)]
        public string Name { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
