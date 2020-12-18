namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class ActionTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ActionTypes.Any())
            {
                return;
            }

            await dbContext.ActionTypes.AddAsync(new ActionType { Name = ActionTypeNames.Loading.ToString() });
            await dbContext.ActionTypes.AddAsync(new ActionType { Name = ActionTypeNames.Unloading.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
