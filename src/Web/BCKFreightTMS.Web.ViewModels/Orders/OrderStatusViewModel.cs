namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class OrderStatusViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        public string OrderToVehicleName { get; set; }

        public string OrderToVehicleRegNumber { get; set; }

        public string OrderToCompanyName { get; set; }

        public string OrderToCompanyComunicatorsMobile1 { get; set; }

        public string OrderToContactFirstName { get; set; }

        public string OrderToContactLastName { get; set; }

        public string OrderToContactComunicatorsMobile1 { get; set; }

        public string OrderToVehicleTrailerReNumber { get; set; }

        public ICollection<KeyValuePair<string, string>> DriversMobiles { get; set; }

        public ICollection<ActionStatusInputModel> Actions { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionTypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionNotFinishedItems { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderStatusViewModel>()
                .ForMember(x => x.DriversMobiles, opt =>
                opt.MapFrom(x => x.OrderTo.Drivers.Select(d =>
                new KeyValuePair<string, string>($"{d.Driver.FirstName} {d.Driver.LastName}", d.Driver.Comunicators.Mobile1))));
        }
    }
}
