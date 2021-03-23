namespace BCKFreightTMS.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    public class BankDetailsModel
    {
        public string CompanyId { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string BankIban { get; set; }
    }
}
