namespace BCKFreightTMS.Web.Controllers
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Vehicles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class VehiclesController : Controller
    {
        private readonly IVehiclesService vehiclesService;

        public VehiclesController(IVehiclesService vehiclesService)
        {
            this.vehiclesService = vehiclesService;
        }

        public IActionResult Index()
        {
            var vehicles = this.vehiclesService.GetAll<VehicleListViewModel>();
            return this.View(vehicles);
        }

        public IActionResult AddVehicle()
        {
            var model = this.vehiclesService.LoadVehicleInputModel();
            return this.View(model);
        }

        public IActionResult AddVehicleModal()
        {
            var model = this.vehiclesService.LoadVehicleInputModel();
            return this.View(model);
        }

        public JsonResult GetDrivers(string companyId)
        {
            var drivers = this.vehiclesService.GetDrivers(companyId);
            return this.Json(drivers);
        }

        public JsonResult GetTrailers(string companyId)
        {
            var trailers = this.vehiclesService.GetTrailers(companyId);
            return this.Json(trailers);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.vehiclesService.LoadVehicleInputModel(input);
                return this.View(input);
            }

            await this.vehiclesService.AddVehicleAsync(input);

            return this.RedirectToAction(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicleModal(VehicleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.vehiclesService.LoadVehicleInputModel(input);
                return this.View(input);
            }

            await this.vehiclesService.AddVehicleAsync(input);

            return this.Json(new { isValid = true, redirectToUrl = string.Empty });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.vehiclesService.DeleteAsync(id))
            {
                return this.RedirectToAction(GlobalConstants.Index);
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }
    }
}
