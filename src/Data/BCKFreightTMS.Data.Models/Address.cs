namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.CompanyAddresses = new HashSet<CompanyAddress>();
            this.CompanyContacts = new HashSet<CompanyContact>();
            this.OrderActions = new HashSet<OrderAction>();
        }

        [MaxLength(100)]
        public string StreetLine { get; set; }

        [MaxLength(20)]
        public string Postcode { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string State { get; set; }

        [MaxLength(200)]
        public string Area { get; set; }

        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

        public virtual ICollection<CompanyContact> CompanyContacts { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
