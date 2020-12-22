namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class CompanyContact : BaseDeletableModel<int>
    {
        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        [Column(TypeName ="varchar(10)")]
        public string Phone { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
