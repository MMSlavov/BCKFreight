﻿namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System.Xml.Serialization;

    public class UNCRAccountMovement
    {
        [XmlIgnore]
        public string Account { get; set; }

        [XmlElement("PaymentDateTime")]
        public string PaymentDateTime { get; set; }

        [XmlElement("ValueDate")]
        public string ValueDate { get; set; }

        [XmlElement("Reason")]
        public string Reason { get; set; }

        [XmlElement("Narrative")]
        public string Narrative { get; set; }

        [XmlElement("NarrativeI02")]
        public string NarrativeI02 { get; set; }

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
