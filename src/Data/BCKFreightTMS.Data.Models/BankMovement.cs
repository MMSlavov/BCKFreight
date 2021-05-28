namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class BankMovement : BaseDeletableModel<string>
    {
        public BankMovement()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime Date { get; set; }

        public string Reason { get; set; }

        public string OppositeSideName { get; set; }

        public string OppositeSideAccount { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey(nameof(AccountingType))]
        public int? AccountingTypeId { get; set; }

        public virtual AccountingType AccountingType { get; set; }

        public virtual ICollection<InvoiceIn> InvoiceIns { get; set; }

        public virtual ICollection<InvoiceOut> InvoiceOuts { get; set; }
    }
}
