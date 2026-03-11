namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            // Sign out and redirect to LogoutComplete page
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            
            // Redirect to LogoutComplete page to clear JWT token
            return this.RedirectToPage("./LogoutComplete");
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Sign out and redirect to LogoutComplete page
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            
            // Redirect to LogoutComplete page to clear JWT token
            return this.RedirectToPage("./LogoutComplete");
        }
    }
}
