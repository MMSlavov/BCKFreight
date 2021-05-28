namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class AccountingType : BaseDeletableModel<int>
    {
        [MaxLength(20)]
        public string Code { get; set; }

        public string Description { get; set; }

        public string MovementType { get; set; }

        public virtual ICollection<BankMovement> BankMovements { get; set; }
    }
}
