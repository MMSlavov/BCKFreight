namespace BCKFreightTMS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "SuperUser")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
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
            foreach (var role in this.roleManager.Roles)
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

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await this.userManager.GetRolesAsync(user));
        }
    }
}
