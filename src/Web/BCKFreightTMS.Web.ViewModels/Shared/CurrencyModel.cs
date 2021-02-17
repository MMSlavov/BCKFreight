namespace BCKFreightTMS.Web.ViewModels.Shared
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CurrencyModel : IMapFrom<Currency>
    {
        public string Name { get; set; }

        public decimal Rate { get; set; }
    }
}
