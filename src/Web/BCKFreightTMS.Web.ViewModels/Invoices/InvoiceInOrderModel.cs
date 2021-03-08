namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System.Collections.Generic;

    using BCKFreightTMS.Web.ViewModels.Orders;

    public class InvoiceInOrderModel
    {
        public string ReferenceNum { get; set; }

        public InvoiceCompanyModel CreatorCompany { get; set; }

        public List<OrderToInvoiceModel> OrderTos { get; set; }

        public InvoiceInModel InvoiceIn { get; set; }
    }
}
