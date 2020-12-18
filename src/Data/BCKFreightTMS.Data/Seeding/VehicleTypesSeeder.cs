namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class VehicleTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.VehicleTypes.Any())
            {
                return;
            }

            await dbContext.VehicleTypes.AddAsync(new VehicleType { Name = VehicleTypeNames.Truck.ToString() });
            await dbContext.VehicleTypes.AddAsync(new VehicleType { Name = VehicleTypeNames.Trailer.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
