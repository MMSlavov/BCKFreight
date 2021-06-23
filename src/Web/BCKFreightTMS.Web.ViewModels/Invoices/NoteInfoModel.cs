namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Common.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class NoteInfoModel
    {
        [Required]
        public InvoiceNoteTypes InvoiceNoteType { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }

        [Required]
        public string Details { get; set; }

        public IEnumerable<SelectListItem> CurrencyItems { get; set; }
    }
}
