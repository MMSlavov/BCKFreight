namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderAction : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey("Order")]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        [ForeignKey("Address")]
        public int AddressId { get; set; }

        public Address Address { get; set; }

        [Required]
        [ForeignKey("Type")]
        public int TypeId { get; set; }

        public ActionType Type { get; set; }

        public DateTime Until { get; set; }

        [Column(TypeName = "text")]
        public string Details { get; set; }
    }
}
