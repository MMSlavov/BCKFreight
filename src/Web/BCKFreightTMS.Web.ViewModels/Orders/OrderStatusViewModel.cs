namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class OrderStatusViewModel : IMapFrom<Order>
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        // public string OrderToVehicleName { get; set; }

        // public string OrderToVehicleRegNumber { get; set; }

        // public string OrderToCompanyName { get; set; }

        // public string OrderToCompanyComunicatorsMobile1 { get; set; }

        // public string OrderToContactFirstName { get; set; }

        // public string OrderToContactLastName { get; set; }

        // public string OrderToContactComunicatorsMobile1 { get; set; }

        // public string OrderToVehicleTrailerRegNumber { get; set; }

        // public ICollection<ActionStatusInputModel> Actions { get; set; }

        // public DocumentationInputModel Documentation { get; set; }
        public List<OrderToModel> OrderTos { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionTypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionNotFinishedItems { get; set; }
    }
}
