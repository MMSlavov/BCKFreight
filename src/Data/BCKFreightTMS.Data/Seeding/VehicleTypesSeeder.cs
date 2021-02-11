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
            foreach (var name in (VehicleTypeNames[])Enum.GetValues(typeof(VehicleTypeNames)))
            {
                if (dbContext.VehicleTypes.Any(s => s.Name == name.ToString()))
                {
                    continue;
                }

                await dbContext.VehicleTypes.AddAsync(new VehicleType { Name = name.ToString() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
