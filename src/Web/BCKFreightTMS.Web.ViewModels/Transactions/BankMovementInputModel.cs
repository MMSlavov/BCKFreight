namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BankMovementInputModel
    {
        public int AccTypeId { get; set; }

        public string OSAccIn { get; set; }

        public string AmountIn { get; set; }

        public string ReasonIn { get; set; }

        public string OSNameIn { get; set; }

        public string OSIdIn { get; set; }

        public DateTime DateIn { get; set; }

        public string[] InvoiceIds { get; set; }
    }
}
