namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListBankMovementModel : IMapFrom<BankMovement>
    {
        public DateTime Date { get; set; }

        public string Reason { get; set; }

        public string OppositeSideName { get; set; }

        public string OppositeSideAccount { get; set; }

        public decimal Amount { get; set; }

        public string AccountingTypeCode { get; set; }

        public string AccountingTypeMovementType { get; set; }

        // public virtual AccountingType AccountingType { get; set; }

        // public virtual ICollection<InvoiceIn> InvoiceIns { get; set; }

        // public virtual ICollection<InvoiceOut> InvoiceOuts { get; set; }
    }
}
