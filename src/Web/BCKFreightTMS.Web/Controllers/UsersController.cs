namespace BCKFreightTMS.Web.Controllers
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
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
        private readonly IUsersService usersService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ApplicationUser> users,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.users = users;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var userRolesViewModel = await this.usersService.GetUserRolesViewModelAsync();

            return this.View(userRolesViewModel);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return this.View("NotFound");
            }

            var model = await this.usersService.GetManageUserViewModelAsync(user, this.User);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ManageUserRolesViewModel model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.Json(new { isValid = false, html = this.View(model) });
            }

            var result = await this.usersService.UpdateUserRolesAsync(model, user);
            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot change user existing roles");
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

            var result = await this.usersService.AddUserAsync(input, this.User);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Json(new { isValid = false, html = this.View(input) });
            }

            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("Index", "Users") });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.usersService.DeleteAsync(id))
            {
                this.ModelState.AddModelError(string.Empty, "User do not exist.");
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }
    }
}
