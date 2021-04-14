namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    public class TaxCountriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            foreach (var name in (TaxCountryNames[])Enum.GetValues(typeof(TaxCountryNames)))
            {
                if (dbContext.TaxCountries.Any(s => s.Name == name.ToString()))
                {
                    continue;
                }

                await dbContext.TaxCountries.AddAsync(new TaxCountry { Name = name.ToString() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
