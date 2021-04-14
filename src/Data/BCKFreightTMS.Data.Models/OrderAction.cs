namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderAction : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(OrderTo))]
        public string OrderToId { get; set; }

        public virtual OrderTo OrderTo { get; set; }

        [ForeignKey(nameof(TaxCountry))]
        public int? TaxCountryId { get; set; }

        public virtual TaxCountry TaxCountry { get; set; }

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

        public bool NoNotes { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Notes { get; set; }

        public virtual ActionNotFinishedReason NotFinishedReason { get; set; }
    }
}
