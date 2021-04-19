namespace BCKFreightTMS.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Shared;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize(Roles = "SuperUser")]
    public partial class CompanyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFinanceService financeService;
        private readonly IDeletableEntityRepository<TaxCountry> taxCountries;
        private readonly IDeletableEntityRepository<Company> companies;

        public CompanyModel(
            UserManager<ApplicationUser> userManager,
            IFinanceService financeService,
            IDeletableEntityRepository<TaxCountry> taxCountries,
            IDeletableEntityRepository<Company> companies)
        {
            this.userManager = userManager;
            this.financeService = financeService;
            this.taxCountries = taxCountries;
            this.companies = companies;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(200)]
            public string Name { get; set; }

            public int? TaxCountryId { get; set; }

            [MaxLength(20)]
            public string TaxNumber { get; set; }

            public string TaxCurrency { get; set; }

            [MaxLength(100)]
            public string StreetLine { get; set; }

            [MaxLength(20)]
            public string Postcode { get; set; }

            [MaxLength(200)]
            public string City { get; set; }

            [MaxLength(200)]
            public string State { get; set; }

            [MaxLength(200)]
            public string Area { get; set; }

            [MaxLength(80)]
            public string MOLFirstName { get; set; }

            [MaxLength(80)]
            public string MOLLastName { get; set; }

            [Required]
            [Phone]
            [MaxLength(10)]
            public string Mobile1 { get; set; }

            [Phone]
            [MaxLength(10)]
            public string Mobile2 { get; set; }

            [Phone]
            [MaxLength(10)]
            public string Phone1 { get; set; }

            [Phone]
            [MaxLength(10)]
            public string Phone2 { get; set; }

            [EmailAddress]
            [MaxLength(50)]
            public string Email1 { get; set; }

            [EmailAddress]
            [MaxLength(10)]
            public string Email2 { get; set; }

            public IEnumerable<SelectListItem> TaxCountryItems { get; set; }

            public ICollection<CurrencyModel> ExchangeRates { get; set; }

            public string Details { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var company = user.Company;
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.LoadAsync(company);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var company = this.companies.All().FirstOrDefault(c => c.Id == user.CompanyId);

            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                this.LoadAsync(company);
                return this.Page();
            }

            company.Name = this.Input.Name;
            company.TaxCountryId = this.Input.TaxCountryId;
            company.TaxNumber = this.Input.TaxNumber;
            company.Address.Address.StreetLine = this.Input.StreetLine;
            company.Address.MOLFirstName = this.Input.MOLFirstName;
            company.Address.MOLLastName = this.Input.MOLLastName;
            company.Comunicators.Mobile1 = this.Input.Mobile1;
            company.Comunicators.Email1 = this.Input.Email1;
            company.Comunicators.Details = this.Input.Details;
            await this.companies.SaveChangesAsync();

            this.StatusMessage = "Your company info has been updated";
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnGetUpdateCurrencyRates()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.financeService.UpdateCurrencyRatesAsync(user.CompanyId);
            this.StatusMessage = "Currency rates has been updated";
            return this.RedirectToPage();
        }

        private void LoadAsync(Company company)
        {
            try
            {
                this.Input = new InputModel
                {
                    Name = company.Name,
                    TaxCountryId = company.TaxCountryId,
                    TaxNumber = company.TaxNumber,
                    TaxCurrency = company.TaxCurrency.Name,
                    StreetLine = company.Address.Address.StreetLine,
                    MOLFirstName = company.Address.MOLFirstName,
                    MOLLastName = company.Address.MOLLastName,
                    Mobile1 = company.Comunicators.Mobile1,
                    Email1 = company.Comunicators.Email1,
                    Details = company.Comunicators.Details,
                    ExchangeRates = this.financeService.GetCurrencyRates(),
                };
                TaxCountryNames res;
                this.Input.TaxCountryItems = this.taxCountries.AllAsNoTracking()
                                            .Select(tc => new SelectListItem(tc.Name, tc.Id.ToString()))
                                            .ToList()
                                            .Where(r => Enum.TryParse<TaxCountryNames>(r.Text, true, out res));
            }
            catch (System.Exception)
            {
            }
        }
    }
}
