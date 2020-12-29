namespace BCKFreightTMS.Data.Configurations
{
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DriverOrderConfiguration : IEntityTypeConfiguration<DriverOrder>
    {
        public void Configure(EntityTypeBuilder<DriverOrder> drOrder)
        {
            drOrder.
                HasKey(dord => new { dord.DriverId, dord.OrderId });
        }
    }
}
