namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InvoiceOutConfiguration : IEntityTypeConfiguration<InvoiceOut>
    {
        public void Configure(EntityTypeBuilder<InvoiceOut> invoiceOut)
        {
            invoiceOut
                .HasOne(i => i.NoteInfo)
                .WithOne(i => i.InvoiceOut)
                .HasForeignKey(typeof(InvoiceOut), "InvoiceNoteId");
        }
    }
}
