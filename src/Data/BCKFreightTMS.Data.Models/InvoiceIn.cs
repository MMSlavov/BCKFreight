namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Models;

    public class InvoiceIn : BaseDeletableModel<string>
    {
        public InvoiceIn()
        {
            this.Id = Guid.NewGuid().ToString();
            this.OrderTos = new HashSet<OrderTo>();
        }

        public string Number { get; set; }

        [ForeignKey(nameof(BankDetails))]
        public int BankDetailsId { get; set; }

        public virtual BankDetails BankDetails { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? PayDate { get; set; }

        public int DueDays { get; set; }

        public string PaymentMethod { get; set; }

        [ForeignKey(nameof(Status))]
        public int? StatusId { get; set; }

        public virtual InvoiceStatus Status { get; set; }

        [ForeignKey(nameof(VATReason))]
        public int? VATReasonId { get; set; }

        public virtual VATReason VATReason { get; set; }

        [ForeignKey(nameof(BankMovement))]
        public string BankMovementId { get; set; }

        public virtual BankMovement BankMovement { get; set; }

        public virtual NoteInfo NoteInfo { get; set; }

        [ForeignKey(nameof(InvoiceNote))]
        public string InvoiceNoteInId { get; set; }

        public virtual InvoiceIn InvoiceNote { get; set; }

        public virtual ICollection<InvoiceIn> NoteInvoices { get; set; }

        public virtual ICollection<OrderTo> OrderTos { get; set; }
    }
}
