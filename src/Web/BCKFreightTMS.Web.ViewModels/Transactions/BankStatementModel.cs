namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    public class BankStatementModel
    {
        public BankAccountMovementModel[] Movements { get; set; }
    }

    public class BankAccountMovementModel
    {
        public string Date { get; set; }

        public string ValueDate { get; set; }

        public string Reason { get; set; }

        public string OppositeSideName { get; set; }

        public string OppositeSideAccount { get; set; }

        public string MovementType { get; set; }

        public decimal Amount { get; set; }
    }
}
