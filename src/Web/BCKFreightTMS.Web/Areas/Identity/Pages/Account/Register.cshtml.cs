namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly ApplicationDbContext dbContext;
        private readonly IFinanceService financeService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext dbContext,
            IFinanceService financeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.dbContext = dbContext;
            this.financeService = financeService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> TaxCurrencyItems { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength(2)]
            [RegularExpression(@"^[A-Za-z ,.'-]+$", ErrorMessage = "Invalid First name.")]
            public string FirstName { get; set; }

            [Required]
            [MinLength(2)]
            [RegularExpression(@"^[A-Za-z ,.'-]+$", ErrorMessage = "Invalid Last name.")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [MaxLength(200)]
            public string CompanyName { get; set; }

            [MaxLength(90)]
            public string TaxCountry { get; set; }

            [MaxLength(20)]
            public string TaxNumber { get; set; }

            public string TaxCurrency { get; set; }

            [MaxLength(200)]
            public string City { get; set; }

            [MaxLength(100)]
            public string StreetLine { get; set; }

            [MaxLength(80)]
            public string MOLFirstName { get; set; }

            [MaxLength(80)]
            public string MOLLastName { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.TaxCurrencyItems = new List<SelectListItem>();
            foreach (var code in (CurrencyCodes[])Enum.GetValues(typeof(CurrencyCodes)))
            {
                this.TaxCurrencyItems.Add(new SelectListItem(code.ToString(), code.ToString()));
            }

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                var company = new Company
                {
                    Name = this.Input.CompanyName,
                    TaxNumber = this.Input.TaxNumber,
                    TaxCurrency = new Currency { Name = this.Input.TaxCurrency },
                    Address = new CompanyAddress
                    {
                        MOLFirstName = this.Input.MOLFirstName,
                        MOLLastName = this.Input.MOLLastName,
                        Address = new Address
                        {
                            StreetLine = this.Input.StreetLine,
                            City = this.Input.City,
                        },
                    },
                    Comunicators = new Comunicators(),
                };

                var taxCoutry = this.dbContext.TaxCountries.FirstOrDefault(c => c.Name == this.Input.TaxCountry);
                if (taxCoutry == null)
                {
                    taxCoutry = new TaxCountry { Name = this.Input.TaxCountry };
                }

                var user = new ApplicationUser
                {
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                    UserName = this.Input.FirstName,
                    Email = this.Input.Email,
                    CompanyId = company.Id,
                };
                user.AdminId = user.Id;
                company.AdminId = user.Id;
                company.TaxCountry = taxCoutry;
                company.Address.AdminId = company.AdminId;
                company.Address.Address.AdminId = company.AdminId;
                company.TaxCountry.AdminId = company.AdminId;
                company.TaxCurrency.AdminId = company.AdminId;
                company.Comunicators.AdminId = company.AdminId;

                await this.financeService.AddNewCompanyCurrencyRatesAsync(company.AdminId, company.TaxCurrency.Name);
                await this.dbContext.Companies.AddAsync(company);
                await this.dbContext.SaveChangesAsync();
                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    await this.userManager.AddToRoleAsync(user, RoleNames.SuperUser.ToString());
                    await this.userManager.AddToRoleAsync(user, RoleNames.User.ToString());

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        this.Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // await this._signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.Page();
        }
    }
}
