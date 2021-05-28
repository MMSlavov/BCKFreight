namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class AccountingTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var name in (CreditAccountingTypes[])Enum.GetValues(typeof(CreditAccountingTypes)))
            {
                if (dbContext.AccountingTypes.Any(t => t.Code == name.ToString()))
                {
                    continue;
                }

                await dbContext.AccountingTypes.AddAsync(new AccountingType { Code = name.ToString(), MovementType = "Credit" });
            }

            foreach (var name in (DebitAccountingTypes[])Enum.GetValues(typeof(DebitAccountingTypes)))
            {
                if (dbContext.AccountingTypes.Any(t => t.Code == name.ToString()))
                {
                    continue;
                }

                await dbContext.AccountingTypes.AddAsync(new AccountingType { Code = name.ToString(), MovementType = "Debit" });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
