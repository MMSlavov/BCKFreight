namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Vehicles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize(Roles = "User")]
    public class VehiclesController : Controller
    {
        private readonly IVehiclesService vehiclesService;
        private readonly IStringLocalizer<VehiclesController> localizer;
        private readonly IDeletableEntityRepository<VehicleType> types;
        private readonly INotyfService notyfService;

        public VehiclesController(
            IVehiclesService vehiclesService,
            IStringLocalizer<VehiclesController> localizer,
            IDeletableEntityRepository<VehicleType> types,
            INotyfService notyfService)
        {
            this.vehiclesService = vehiclesService;
            this.localizer = localizer;
            this.types = types;
            this.notyfService = notyfService;
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

        public IActionResult AddTruckModal(string id)
        {
            var model = this.vehiclesService.LoadVehicleInputModel();
            this.ViewBag.TrailerItems = this.vehiclesService.GetTrailers(id);
            model.TypeId = this.types.AllAsNoTracking().FirstOrDefault(t => t.Name == VehicleTypeNames.Truck.ToString()).Id;
            model.CompanyId = id;
            return this.View(model);
        }

        public IActionResult AddSoloModal(string id)
        {
            var model = this.vehiclesService.LoadVehicleInputModel();
            this.ViewBag.DriverItems = this.vehiclesService.GetDrivers(id);
            model.TypeId = this.types.AllAsNoTracking().FirstOrDefault(t => t.Name == VehicleTypeNames.Solo.ToString()).Id;
            model.CompanyId = id;
            return this.View(model);
        }

        public IActionResult AddTrailerModal(string id)
        {
            var model = this.vehiclesService.LoadVehicleInputModel();
            model.TypeId = this.types.AllAsNoTracking().FirstOrDefault(t => t.Name == VehicleTypeNames.Trailer.ToString()).Id;
            model.CompanyId = id;
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

            try
            {
                await this.vehiclesService.AddVehicleAsync(input);
            }
            catch (Exception ex)
            {
                this.notyfService.Error(this.localizer[ex.Message]);
                input = this.vehiclesService.LoadVehicleInputModel(input);
                return this.View(input);
            }

            return this.Json(new { isValid = true, redirectToUrl = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> AddTruckModal(VehicleInputModel input)
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
