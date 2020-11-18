namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Company : BaseDeletableModel<string>
    {
        public Company()
        {
            this.Id = Guid.NewGuid().ToString();
            this.People = new HashSet<Person>();
            this.Contacts = new HashSet<CompanyContact>();
            this.OrderTos = new HashSet<OrderTo>();
            this.OrderFroms = new HashSet<OrderFrom>();
        }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [ForeignKey(nameof(TaxCountry))]
        public int TaxCountryId { get; set; }

        public TaxCountry TaxCountry { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string TaxNumber { get; set; }

        public int CompanyAddressId { get; set; }

        public CompanyAddress Address { get; set; }

        public int ComunicatorsId { get; set; }

        public Comunicators Comunicators { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<CompanyContact> Contacts { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }

        public virtual ICollection<OrderFrom> OrderFroms { get; set; }
    }
}
