namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class OrderToListModel : IMapFrom<OrderTo>
    {
        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string CarrierOrderCompanyName { get; set; }

        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }

        public decimal PriceNetOut { get; set; }

        public int CurrencyOutId { get; set; }

        public string CargoName { get; set; }
    }
}
