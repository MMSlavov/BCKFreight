namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("AccountMovementsResult")]
    public class DSKStatementModel
    {
        [XmlElement("AccountMovementsFilter")]
        public AccountMovementsFilter AccountMovementsFilter { get; set; }

        [XmlElement("Sums")]
        public Sums Sums { get; set; }

        [XmlElement("Bank")]
        public Bank Bank { get; set; }

        [XmlElement("Client")]
        public Client Client { get; set; }

        [XmlArray("AccountMovements")]
        [XmlArrayItem("AccountMovement")]
        public AccountMovement[] AccountMovements { get; set; }
    }

    [Serializable]
    public class AccountMovementsFilter
    {
        [XmlElement("BankAccountID")]
        public string BankAccountID { get; set; }

        [XmlElement("StartDate")]
        public string StartDate { get; set; }

        [XmlElement("EndDate")]
        public string EndDate { get; set; }
    }

    [Serializable]
    public class Sums
    {
        [XmlElement("BeginSum", DataType = "decimal")]
        public decimal BeginSum { get; set; }

        [XmlElement("TurnoverCR", DataType = "decimal")]
        public decimal TurnoverCR { get; set; }

        [XmlElement("TurnoverDR", DataType = "decimal")]
        public decimal TurnoverDR { get; set; }

        [XmlElement("EndSum", DataType = "decimal")]
        public decimal EndSum { get; set; }
    }

    [Serializable]
    public class Bank
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Address")]
        public string Address { get; set; }

        [XmlElement("BIC")]
        public string BIC { get; set; }
    }

    [Serializable]
    public class Client
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("PersonalIdBulstat")]
        public string PersonalIdBulstat { get; set; }

        [XmlElement("Address")]
        public string Address { get; set; }
    }

    [Serializable]
    public class AccountMovement
    {
        [XmlElement("AccountingDate")]
        public string AccountingDate { get; set; }

        [XmlElement("ValueDate")]
        public string ValueDate { get; set; }

        [XmlElement("Reason")]
        public string Reason { get; set; }

        [XmlElement("OppositeSideName")]
        public string OppositeSideName { get; set; }

        [XmlElement("OppositeSideAccount")]
        public string OppositeSideAccount { get; set; }

        [XmlElement("MovementType")]
        public string MovementType { get; set; }

        [XmlElement("Amount", DataType = "decimal")]
        public decimal Amount { get; set; }
    }
}
