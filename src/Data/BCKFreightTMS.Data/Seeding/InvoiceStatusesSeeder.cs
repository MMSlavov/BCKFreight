namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class InvoiceStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var name in (InvoiceStatusNames[])Enum.GetValues(typeof(InvoiceStatusNames)))
            {
                if (dbContext.InvoiceStatuses.Any(s => s.Name == name.ToString()))
                {
                    continue;
                }

                await dbContext.InvoiceStatuses.AddAsync(new InvoiceStatus { Name = name.ToString() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
