namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;

    internal class PersonRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PersonRoles.Any())
            {
                return;
            }

            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleNames.Employee.ToString() });
            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleNames.Driver.ToString() });
            await dbContext.PersonRoles.AddAsync(new PersonRole { Name = PersonRoleNames.Contact.ToString() });

            await dbContext.SaveChangesAsync();
        }
    }
}
