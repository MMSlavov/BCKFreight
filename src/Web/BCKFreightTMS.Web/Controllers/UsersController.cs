namespace BCKFreightTMS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "SuperUser")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDeletableEntityRepository<ApplicationUser> users;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ApplicationUser> users)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.users = users;
        }

        public async Task<IActionResult> Index()
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
                thisViewModel.Roles = await this.GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return this.View(userRolesViewModel);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            var model = new ManageUserRolesViewModel();
            model.UserId = userId;

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return this.View("NotFound");
            }

            model.UserName = user.UserName;
            model.RoleModels = new List<UserRoleInputModel>();
            var roles = this.roleManager.Roles.ToList();
            if (!this.User.IsInRole(RoleNames.Admin.ToString()))
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

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ManageUserRolesViewModel model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.Json(new { isValid = false, html = this.View() });
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var result = await this.userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
                return this.Json(new { isValid = false, html = this.View(model) });
            }

            result = await this.userManager.AddToRolesAsync(user, model.RoleModels.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot add selected roles to user");
                return this.Json(new { isValid = false, html = this.View(model) });
            }

            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("Index", "Users") });
        }

        public IActionResult AddUser()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View(input) });
            }

            var user = new ApplicationUser
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.FirstName,
                Email = input.Email,
            };
            user.AdminId = this.userManager.GetUserId(this.User);
            var result = await this.userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Json(new { isValid = false, html = this.View(input) });
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

            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("Index", "Users") });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user is null)
            {
                this.ModelState.AddModelError(string.Empty, "User do not exist.");
            }

            this.users.Delete(user);
            await this.users.SaveChangesAsync();

            return this.RedirectToAction(GlobalConstants.Index);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await this.userManager.GetRolesAsync(user));
        }
    }
}
