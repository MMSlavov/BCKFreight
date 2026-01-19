namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Identity;

    public interface IUsersService
    {
        public Task<List<UserRolesViewModel>> GetUserRolesViewModelAsync();

        public Task<ManageUserRolesViewModel> GetManageUserViewModelAsync(ApplicationUser user, ClaimsPrincipal adminUser);

        public Task<IdentityResult> UpdateUserRolesAsync(ManageUserRolesViewModel model, ApplicationUser user);

        public Task<IdentityResult> AddUserAsync(AddUserInputModel input, ClaimsPrincipal adminUser);

        public Task<bool> DeleteAsync(string id);

        public Task<bool> UpdatePreferredLanguageAsync(string userId, string language);
    }
}
