namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System.Collections.Generic;

    public class InvoiceOutInputModel
    {
        public InvoiceOutModel InvoiceOut { get; set; }

        public InvoiceCompanyModel OrderCreatorCompany { get; set; }

        public InvoiceCompanyModel OrderOrderFromCompany { get; set; }

        public int OrderDueDaysFrom { get; set; }

        public string SelectedReasonId { get; set; }

        public List<OrderToInvoiceModel> OrderTos { get; set; }
    }
}
