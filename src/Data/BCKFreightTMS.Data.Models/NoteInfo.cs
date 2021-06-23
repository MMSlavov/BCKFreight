namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Models;

    public class NoteInfo : BaseDeletableModel<int>
    {
        public InvoiceNoteTypes? InvoiceNoteType { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey(nameof(Currency))]
        public int? CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public string Details { get; set; }

        public virtual InvoiceIn InvoiceIn { get; set; }

        public virtual InvoiceOut InvoiceOut { get; set; }
    }
}
