namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class LogoutCompleteModel : PageModel
    {
        public void OnGet()
        {
            // Just render the page - cookie auth already cleared
        }
    }
}
