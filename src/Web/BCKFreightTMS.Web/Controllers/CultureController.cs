namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    public class CultureController : Controller
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public CultureController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SetLanguage(string culture, string returnUrl)
        {
            this.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true
                });

            if (this.User.Identity.IsAuthenticated)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                if (user != null)
                {
                    await this.usersService.UpdatePreferredLanguageAsync(user.Id, culture);
                }
            }

            return this.LocalRedirect(returnUrl);
        }
    }
}
