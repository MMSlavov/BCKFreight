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
            if (dbContext.OrderStatuses.Any())
            {
                return;
            }

            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = OrderStatusNames.Draft.ToString() });
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = OrderStatusNames.InProgress.ToString() });
            await dbContext.OrderStatuses.AddAsync(new OrderStatus { Name = OrderStatusNames.Finished.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
