namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("Items")]
    public class UNCRStatementModel
    {
        [XmlElement("AccountMovement")]
        public UNCRAccountMovement[] Items { get; set; }
    }
}
