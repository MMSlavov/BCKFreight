namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class CurrencyCodesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Currency.Any())
            {
                return;
            }

            await dbContext.Currency.AddAsync(new Currency { Name = CurrencyCodes.BGN.ToString() });
            await dbContext.Currency.AddAsync(new Currency { Name = CurrencyCodes.RON.ToString() });
            await dbContext.Currency.AddAsync(new Currency { Name = CurrencyCodes.EUR.ToString() });
            await dbContext.Currency.AddAsync(new Currency { Name = CurrencyCodes.SRD.ToString() });
            await dbContext.Currency.AddAsync(new Currency { Name = CurrencyCodes.TRY.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
