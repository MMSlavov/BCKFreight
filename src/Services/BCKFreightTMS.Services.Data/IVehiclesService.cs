namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Vehicles;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IVehiclesService
    {
        public IEnumerable<T> GetAll<T>();

        public VehicleInputModel LoadVehicleInputModel(VehicleInputModel model = null);

        public IEnumerable<SelectListItem> GetDrivers(string companyId);

        public IEnumerable<SelectListItem> GetTrailers(string companyId);

        public Task<string> AddVehicleAsync(VehicleInputModel input);

        public Task<bool> DeleteAsync(string id);
    }
}
