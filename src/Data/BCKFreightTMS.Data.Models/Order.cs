namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        [MaxLength(15)]
        public string ReferenceNum { get; set; }

        public int? OrderFromId { get; set; }

        public virtual OrderFrom OrderFrom { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public int? InvoiceInId { get; set; }

        public virtual InvoiceIn InvoiceIn { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string FailReason { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }
    }
}
