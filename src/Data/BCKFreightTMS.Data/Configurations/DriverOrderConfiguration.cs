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
                HasKey(drOrder => new { drOrder.DriverId, drOrder.OrderId });
        }
    }
}
