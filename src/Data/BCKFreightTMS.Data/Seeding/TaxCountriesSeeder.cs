namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class OrderStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var name in (OrderStatusNames[])Enum.GetValues(typeof(OrderStatusNames)))
            {
                if (dbContext.OrderStatuses.Any(s => s.Name == name.ToString()))
                {
                    continue;
                }

                await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = name.ToString() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
