namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderFrom : BaseDeletableModel<int>
    {
        public string ReferenceNum { get; set; }

        public decimal PriceNetIn { get; set; }

        public int DueDays { get; set; }

        [Required]
        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public OrderType Type { get; set; }

        [ForeignKey(nameof(Contact))]
        public string ContactId { get; set; }

        public Person Contact { get; set; }

        public Order Order { get; set; }
    }
}
