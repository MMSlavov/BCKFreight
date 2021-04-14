namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.Collections.Generic;

    public class InvoiceModel
    {
        public string ReturnUrl { get; set; }

        public string Id { get; set; }

        public string StatusName { get; set; }

        public string Number { get; set; }

        public string BankDetailsBankCode { get; set; }

        public string BankDetailsBankIban { get; set; }

        public string VATReasonName { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int DueDays { get; set; }

        public string PaymentMethod { get; set; }

        public InvoiceCompanyModel CreatorCompany { get; set; }

        public InvoiceCompanyModel ClientCompany { get; set; }

        public List<OrderToInvoiceModel> OrderTos { get; set; }
    }
}
