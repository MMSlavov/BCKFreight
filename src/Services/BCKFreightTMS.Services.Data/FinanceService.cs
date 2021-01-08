namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Newtonsoft.Json.Linq;

    public class FinanceService : IFinanceService
    {
        private readonly IDeletableEntityRepository<Currency> currencies;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly ApplicationDbContext dbContext;

        public FinanceService(
            IDeletableEntityRepository<Currency> currencies,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Company> companies,
            ApplicationDbContext dbContext)
        {
            this.currencies = currencies;
            this.userManager = userManager;
            this.companies = companies;
            this.dbContext = dbContext;
        }

        public async Task AddNewCompanyCurrencyRatesAsync(string adminId, string companyCurrency)
        {
            foreach (var code in (CurrencyCodes[])Enum.GetValues(typeof(CurrencyCodes)))
            {
                var codeStr = code.ToString();
                if (codeStr != companyCurrency)
                {
                    await this.dbContext.Currency.AddAsync(new Currency { Name = codeStr, AdminId = adminId });
                }
            }
        }

        public decimal GetAmount(int currencyId, decimal amount)
        {
            var currency = this.currencies.All().FirstOrDefault(c => c.Id == currencyId);

            return amount * currency.Rate;
        }

        public async Task<decimal> GetAmountForUserAsync(ClaimsPrincipal userCP, decimal amount, int fromCurrencyId = 0)
        {
            var user = await this.userManager.GetUserAsync(userCP);
            var companyCurrency = user.Company.TaxCurrency;
            if (fromCurrencyId != 0)
            {
                amount = this.GetAmount(fromCurrencyId, amount);
            }

            return this.GetAmount(companyCurrency.Id, amount);
        }

        public async Task UpdateCurrencyRatesAsync(string companyId)
        {
            var currencies = this.currencies.All().ToList();
            var companyCurr = this.companies.All().FirstOrDefault(c => c.Id == companyId).TaxCurrency;

            foreach (var currency in currencies)
            {
                var rate = this.GetCurrencyExRate(currency.Name, companyCurr.Name);
                currency.Rate = rate;
            }

            await this.currencies.SaveChangesAsync();
        }

        private decimal GetCurrencyExRate(string fromCurrency, string toCurrency)
        {
            string url = string.Format(GlobalConstants.ExchangeRateUrl, fromCurrency, toCurrency);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                JToken token = JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return exchangeRate;
            }
        }
    }
}
