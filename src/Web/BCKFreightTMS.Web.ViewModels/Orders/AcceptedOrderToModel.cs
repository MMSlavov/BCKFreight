namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class AcceptedOrderToModel : IMapFrom<OrderTo>
    {
        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }
    }
}
