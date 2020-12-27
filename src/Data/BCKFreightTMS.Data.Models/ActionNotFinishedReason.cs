namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class ActionNotFinishedReason : SettingModel
    {
        public ActionNotFinishedReason()
        {
            this.OrderActions = new HashSet<OrderAction>();
        }

        [Required]
        [MaxLength(200)]
        public override string Name { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
