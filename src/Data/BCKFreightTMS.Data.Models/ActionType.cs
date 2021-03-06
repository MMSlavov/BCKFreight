﻿namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class ActionType : BaseDeletableModel<int>
    {
        public ActionType()
        {
            this.OrderActions = new HashSet<OrderAction>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
