namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class InvoiceInModel
    {
        public string Number { get; set; }

        public int BankDetailsId { get; set; }

        public DateTime ReceiveDate { get; set; }

        public int DueDays { get; set; }

        public string PaymentMethod { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodItems { get; set; }

        public IEnumerable<SelectListItem> BankDetailsItems { get; set; }
    }
}
