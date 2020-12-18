namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class LoadingBodiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.VehicleLoadingBodies.Any())
            {
                return;
            }

            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Tarpaulin.ToString() });
            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Refrigeration.ToString() });
            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Hanger.ToString() });
            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Onboard.ToString() });
            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Mega.ToString() });
            await dbContext.VehicleLoadingBodies.AddAsync(new VehicleLoadingBody { Name = LoadingBodyNames.Gondola.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
