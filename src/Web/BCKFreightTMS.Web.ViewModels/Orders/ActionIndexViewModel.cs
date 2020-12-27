namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ActionIndexViewModel : IMapFrom<OrderAction>
    {
        public string OrderId { get; set; }

        public string OrderStatusName { get; set; }

        public string TypeName { get; set; }

        public string AddressCity { get; set; }

        public string AddressStreetLine { get; set; }

        public DateTime Until { get; set; }

        public bool IsFinished { get; set; }
    }
}
