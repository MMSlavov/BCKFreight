namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListInvoiceInModel : IMapFrom<InvoiceIn>
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        public string Number { get; set; }

        public string BankDetailsCompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReceiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime PayDate { get; set; }

        public int DueDays { get; set; }

        public bool NoVAT { get; set; }

        public string VATReasonName { get; set; }

        public decimal Price { get; set; }

        public List<OrderToInvoiceModel> OrderTos { get; set; }
    }
}
