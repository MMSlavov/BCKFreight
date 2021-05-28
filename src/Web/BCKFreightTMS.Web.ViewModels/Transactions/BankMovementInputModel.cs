namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System;

    public class BankMovementInputModel
    {
        public int AccTypeId { get; set; }

        public string OSAccIn { get; set; }

        public decimal AmountIn { get; set; }

        public string ReasonIn { get; set; }

        public string OSNameIn { get; set; }

        public DateTime DateIn { get; set; }
    }
}
