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
            this.OrderActions = new HashSet<OrderAction>();
        }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        [Required]
        [ForeignKey(nameof(Cargo))]
        public string CargoId { get; set; }

        public virtual Cargo Cargo { get; set; }

        public int OrderToId { get; set; }

        public virtual OrderTo OrderTo { get; set; }

        public int OrderFromId { get; set; }

        public virtual OrderFrom OrderFrom { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public int? DocumentationId { get; set; }

        [ForeignKey(nameof(DocumentationId))]
        public virtual Documentation Documentation { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
