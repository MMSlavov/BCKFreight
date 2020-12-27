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

        public virtual Order Order { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        [Required]
        [ForeignKey(nameof(Type))]
        public int TypeId { get; set; }

        public virtual ActionType Type { get; set; }

        public DateTime Until { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Details { get; set; }

        public bool IsFinished { get; set; }

        [ForeignKey(nameof(NotFinishedReason))]
        public int? NotFinishedReasonId { get; set; }

        public virtual ActionNotFinishedReason NotFinishedReason { get; set; }
    }
}
