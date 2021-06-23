namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InvoiceInConfiguration : IEntityTypeConfiguration<InvoiceIn>
    {
        public void Configure(EntityTypeBuilder<InvoiceIn> invoiceIn)
        {
            invoiceIn
                .HasOne(i => i.NoteInfo)
                .WithOne(i => i.InvoiceIn)
                .HasForeignKey(typeof(InvoiceIn), "InvoiceNoteId");
        }
    }
}
