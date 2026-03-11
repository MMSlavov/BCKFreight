namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account
{
    using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class StoreTokenModel : PageModel
    {
        public string JwtToken { get; set; }

        public string JwtExpiration { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            this.JwtToken = this.TempData["JwtToken"]?.ToString();
            this.JwtExpiration = this.TempData["JwtExpiration"] is DateTime dt
                ? dt.ToString("o")
                : this.TempData["JwtExpiration"]?.ToString();
            this.UserId = this.TempData["UserId"]?.ToString();
            this.Username = this.TempData["Username"]?.ToString();
            this.ReturnUrl = this.TempData["ReturnUrl"]?.ToString() ?? "/";

            if (string.IsNullOrEmpty(this.JwtToken))
            {
                return this.LocalRedirect(this.ReturnUrl);
            }

            return this.Page();
        }
    }
}
