namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class BankDetails : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey("Company")]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string BankCode { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(22)]
        public string BankIban { get; set; }
    }
}
