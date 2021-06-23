namespace BCKFreightTMS.Web.ViewModels.Shared
{
    using System;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CurrencyModel : IMapFrom<Currency>
    {
        public string Name { get; set; }

        public decimal Rate { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
