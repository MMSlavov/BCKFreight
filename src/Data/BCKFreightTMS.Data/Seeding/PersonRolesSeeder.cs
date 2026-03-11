namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    internal class PersonRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider = null)
        {
            if (dbContext.PersonRoles.Any())
            {
                return;
            }

            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleName.Employee.ToString() });
            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleName.Driver.ToString() });
            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleName.Contact.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
