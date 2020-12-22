namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class CompanyAddress : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        [MaxLength(80)]
        public string MOLFirstName { get; set; }

        [MaxLength(80)]
        public string MOLLastName { get; set; }

        public virtual Company Company { get; set; }
    }
}
