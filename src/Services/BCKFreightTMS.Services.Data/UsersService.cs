namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;

    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDeletableEntityRepository<ApplicationUser> users;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ApplicationUser> users)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.users = users;
        }

        public async Task<List<UserRolesViewModel>> GetUserRolesViewModelAsync()
        {
            var users = this.users.All()
                                  .OrderByDescending(u => u.Roles.Count)
                                  .ThenBy(u => u.FirstName)
                                  .ToList();

            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await this.GetUserRolesAsync(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return userRolesViewModel;
        }

        public async Task<ManageUserRolesViewModel> GetManageUserViewModelAsync(ApplicationUser user, ClaimsPrincipal adminUser)
        {
            var model = new ManageUserRolesViewModel();
            model.UserId = user.Id;

            model.UserName = user.UserName;
            model.RoleModels = new List<UserRoleInputModel>();
            var roles = this.roleManager.Roles.ToList();
            if (!adminUser.IsInRole(RoleNames.Admin.ToString()))
            {
                roles.Remove(roles.First(r => r.Name == RoleNames.Admin.ToString()));
            }

            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleInputModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                if (await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                model.RoleModels.Add(userRolesViewModel);
            }

            return model;
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(ManageUserRolesViewModel model, ApplicationUser user)
        {
            var roles = await this.userManager.GetRolesAsync(user);
            var result = await this.userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                return result;
            }

            return await this.userManager.AddToRolesAsync(user, model.RoleModels.Where(x => x.Selected).Select(y => y.RoleName));
        }

        public async Task<IdentityResult> AddUserAsync(AddUserInputModel input, ClaimsPrincipal adminUser)
        {
            var user = new ApplicationUser
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.FirstName,
                Email = input.Email,
            };
            var admin = await this.userManager.GetUserAsync(adminUser);
            user.AdminId = admin.Id;
            user.CompanyId = admin.CompanyId;
            var result = await this.userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            // var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            // var callbackUrl = this.Url.Page(
            //    "/Account/ConfirmEmail",
            //    pageHandler: null,
            //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
            //    protocol: this.Request.Scheme);

            // await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            await this.userManager.AddToRoleAsync(user, RoleNames.User.ToString());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user is null)
            {
                return false;
            }

            this.users.Delete(user);
            await this.users.SaveChangesAsync();

            return true;
        }

        private async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return new List<string>(await this.userManager.GetRolesAsync(user));
        }
    }
}
