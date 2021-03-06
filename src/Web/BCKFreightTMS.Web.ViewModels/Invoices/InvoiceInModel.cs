﻿namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class InvoiceInModel : IMapFrom<InvoiceIn>
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public int BankDetailsId { get; set; }

        [Required]
        public int VATReasonId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReceiveDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int DueDays { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodItems { get; set; }

        public IEnumerable<SelectListItem> BankDetailsItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ReasonNoVATItems { get; set; }
    }
}
