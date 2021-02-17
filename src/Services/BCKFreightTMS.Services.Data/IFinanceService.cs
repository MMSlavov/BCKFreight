namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Shared;

    public interface IFinanceService
    {
        public ICollection<CurrencyModel> GetCurrencyRates();

        public Task UpdateCurrencyRatesAsync(string companyId);

        public decimal GetAmount(int currencyId, decimal amount);

        public Task<decimal> GetAmountForUserAsync(ClaimsPrincipal userCP, decimal amount, int fromCurrencyId = 0);

        public Task AddNewCompanyCurrencyRatesAsync(string adminId, string companyCurrency);
    }
}
