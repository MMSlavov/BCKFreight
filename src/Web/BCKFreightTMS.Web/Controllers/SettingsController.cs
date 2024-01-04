namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Settings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "SuperUser")]
    public class SettingsController : BaseController
    {
        private readonly IDeletableEntityRepository<PersonRole> perRoles;
        private readonly IDeletableEntityRepository<CargoType> cargoTypes;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadBodies;
        private readonly ISettingsService settingsService;

        public SettingsController(
            IDeletableEntityRepository<PersonRole> perRoles,
            IDeletableEntityRepository<CargoType> cargoTypes,
            IDeletableEntityRepository<VehicleLoadingBody> loadBodies,
            ISettingsService settingsService)
        {
            this.perRoles = perRoles;
            this.cargoTypes = cargoTypes;
            this.loadBodies = loadBodies;
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var model = this.perRoles.AllAsNoTracking().To<SettingViewModel>().ToList();
            return this.View(model);
        }

        public IActionResult AddPersonRole()
        {
            var model = new SettingInputModel { Action = nameof(this.AddPersonRole) };
            return this.View("AddSetting", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonRole(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View("AddSetting", input) });
            }

            await this.settingsService.AddPersonRoleAsync(input);
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("Index", "Settings") });
        }

        public async Task<IActionResult> DeletePersonRole(int id)
        {
            await this.settingsService.DeletePersonRoleAsync(id);

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult CargoTypes()
        {
            var model = this.cargoTypes.AllAsNoTracking().To<SettingViewModel>().ToList();
            return this.View(model);
        }

        [HttpGet("/CargoTypes/AddCargoType")]
        public IActionResult AddCargoType()
        {
            var model = new SettingInputModel { Action = nameof(this.AddCargoType) };
            return this.View("AddSetting", model);
        }

        [HttpPost("/CargoTypes/AddCargoType")]
        public async Task<IActionResult> AddCargoType(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View("AddSetting", input) });
            }

            await this.settingsService.AddCargoTypeAsync(input);
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("CargoTypes", "Settings") });
        }

        public async Task<IActionResult> DeleteCargoType(int id)
        {
            await this.settingsService.DeleteCargoTypeAsync(id);

            return this.RedirectToAction("CargoTypes");
        }

        public IActionResult LoadingBodies()
        {
            var model = this.loadBodies.AllAsNoTracking().To<SettingViewModel>().ToList();
            return this.View(model);
        }

        [HttpGet("/LoadingBodies/AddLoadingBody")]
        public IActionResult AddLoadingBody()
        {
            var model = new SettingInputModel { Action = nameof(this.AddLoadingBody) };
            return this.View("AddSetting", model);
        }

        [HttpPost("/LoadingBodies/AddLoadingBody")]
        public async Task<IActionResult> AddLoadingBody(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View("AddSetting", input) });
            }

            await this.settingsService.AddLoadingBodyAsync(input);
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("LoadingBodies", "Settings") });
        }

        public async Task<IActionResult> DeleteLoadingBody(int id)
        {
            await this.settingsService.DeleteLoadingBodyAsync(id);

            return this.RedirectToAction("LoadingBodies");
        }
    }
}
