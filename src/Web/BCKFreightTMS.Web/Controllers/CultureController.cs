namespace BCKFreightTMS.Web.Controllers
{
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    public class CultureController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            this.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));

            return this.LocalRedirect(returnUrl);
        }
    }
}
