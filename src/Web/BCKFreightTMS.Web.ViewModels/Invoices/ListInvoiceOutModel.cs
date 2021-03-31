namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListInvoiceOutModel : IMapFrom<InvoiceOut>
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        public string Number { get; set; }

        public string BankDetailsCompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int DueDays { get; set; }
    }
}
