namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CarrierOrderListModel : IMapFrom<CarrierOrder>
    {
        public string Id { get; set; }

        public string ReferenceNum { get; set; }
    }
}
