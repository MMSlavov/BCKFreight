namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System.Collections.Generic;

    public class InvoiceInInputModel
    {
        public InvoiceInModel InvoiceIn { get; set; }

        public InvoiceCompanyModel OrderCreatorCompany { get; set; }

        public InvoiceCompanyModel CarrierOrderCompany { get; set; }

        public int OrderDueDaysTo { get; set; }

        public string SelectedReasonId { get; set; }

        public List<OrderToInvoiceModel> OrderTos { get; set; }
    }
}
