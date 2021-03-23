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

        public int DueDaysFrom { get; set; }

        public int DueDaysTo { get; set; }

        public int? OrderFromId { get; set; }

        public virtual OrderFrom OrderFrom { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string FailReason { get; set; }

        public virtual ICollection<CarrierOrder> CarrierOrders { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }
    }
}
