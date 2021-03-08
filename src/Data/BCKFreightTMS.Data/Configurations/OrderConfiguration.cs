namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> order)
        {
            order
                .HasOne(o => o.OrderFrom)
                .WithOne(ot => ot.Order)
                .HasForeignKey<Order>(o => o.OrderFromId);

            order
                .HasOne(o => o.InvoiceIn)
                .WithOne(i => i.Order)
                .HasForeignKey<Order>(o => o.InvoiceInId);
        }
    }
}
