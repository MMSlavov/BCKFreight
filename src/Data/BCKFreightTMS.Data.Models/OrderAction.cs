namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderAction : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        [Required]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public ActionType Type { get; set; }

        public DateTime Until { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Details { get; set; }
    }
}
