namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class InvoiceIn : BaseDeletableModel<int>
    {
        public string Number { get; set; }

        [ForeignKey(nameof(BankDetails))]
        public int BankDetailsId { get; set; }

        public virtual BankDetails BankDetails { get; set; }

        public DateTime ReceiveDate { get; set; }

        public int DueDays { get; set; }

        public string PaymentMethod { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
