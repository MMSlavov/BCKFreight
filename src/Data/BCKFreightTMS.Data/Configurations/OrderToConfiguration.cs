namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderToConfiguration : IEntityTypeConfiguration<OrderTo>
    {
        public void Configure(EntityTypeBuilder<OrderTo> orderTo)
        {
            orderTo
                .HasOne(o => o.Cargo)
                .WithOne(ot => ot.OrderTo)
                .HasForeignKey<OrderTo>(o => o.CargoId);
        }
    }
}
