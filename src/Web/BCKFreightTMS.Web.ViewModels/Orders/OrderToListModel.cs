namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class OrderToListModel : IMapFrom<OrderTo>
    {
        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string CompanyName { get; set; }

        public decimal PriceNetOut { get; set; }

        public int CurrencyId { get; set; }

        public string ReferenceNum { get; set; }

        public string CargoName { get; set; }
    }
}
