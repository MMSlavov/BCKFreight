namespace BCKFreightTMS.Web.Infrastructure
{
    using System.Threading.Tasks;
    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;

    public class UserPreferredCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userManager = httpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
                if (userManager != null)
                {
                    var user = await userManager.GetUserAsync(httpContext.User);
                    if (user?.PreferredLanguage != null)
                    {
                        return new ProviderCultureResult(user.PreferredLanguage);
                    }
                }
            }

            return null;
        }
    }
}
