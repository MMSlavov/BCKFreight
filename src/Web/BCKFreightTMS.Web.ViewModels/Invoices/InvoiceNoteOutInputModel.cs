namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InvoiceNoteOutInputModel
    {
        public InvoiceNoteOutInputModel()
        {
            this.Note = new NoteInfoModel();
        }

        [Required]
        public string Number { get; set; }

        [Required]
        public int BankDetailsId { get; set; }

        [Required]
        public int VATReasonId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int DueDays { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public InvoiceOutModel InvoiceOut { get; set; }

        public InvoiceCompanyModel OrderCreatorCompany { get; set; }

        public InvoiceCompanyModel OrderOrderFromCompany { get; set; }

        public int OrderDueDaysFrom { get; set; }

        public string SelectedReasonId { get; set; }

        public NoteInfoModel Note { get; set; }
    }
}
