namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class VATReasonsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var name in (VATReasons[])Enum.GetValues(typeof(VATReasons)))
            {
                if (dbContext.VATReasons.Any(s => s.Name == name.ToString()))
                {
                    continue;
                }

                await dbContext.VATReasons.AddAsync(new VATReason { Name = name.ToString() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
