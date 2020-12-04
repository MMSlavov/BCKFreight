namespace BCKFreightTMS.Web.ViewModels.Vehicles
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class VehicleListViewModel : IMapFrom<Vehicle>
    {
        public string Id { get; set; }

        public string TypeName { get; set; }

        public string LoadingBodyName { get; set; }

        public string CompanyName { get; set; }

        public string RegNumber { get; set; }

        public string Name { get; set; }
    }
}
