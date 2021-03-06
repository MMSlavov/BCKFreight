﻿namespace BCKFreightTMS.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var defaultUser = new ApplicationUser
            {
                UserName = "MMSlavov",
                Email = "admin@gmail.com",
                FirstName = "Mario",
                LastName = "Slavov",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                ProfilePicture = null,
                Company = new Company
                {
                    Name = "AdminCompany",
                    TaxNumber = "00000000000",
                    TaxCurrency = new Currency { Name = "BGN" },
                    TaxCountry = new TaxCountry { Name = "Admin country" },
                    Address = new CompanyAddress
                    {
                        MOLFirstName = "Admin",
                        MOLLastName = "Admin",
                        Address = new Address
                        {
                            StreetLine = "Admin street",
                            City = "Admin city",
                        },
                    },
                    Comunicators = new Comunicators(),
                },
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    defaultUser.Company.AdminId = defaultUser.Id;
                    defaultUser.Company.Address.AdminId = defaultUser.Id;
                    defaultUser.Company.Address.Address.AdminId = defaultUser.Id;
                    defaultUser.Company.TaxCountry.AdminId = defaultUser.Id;
                    defaultUser.Company.TaxCurrency.AdminId = defaultUser.Id;
                    defaultUser.Company.Comunicators.AdminId = defaultUser.Id;
                    defaultUser.Company.AdminId = defaultUser.Id;
                    defaultUser.AdminId = defaultUser.Id;
                    await userManager.CreateAsync(defaultUser, "22XfMYvQunLU8sWD");
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.SuperUser.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.Admin.ToString());
                }
            }
        }
    }
}
