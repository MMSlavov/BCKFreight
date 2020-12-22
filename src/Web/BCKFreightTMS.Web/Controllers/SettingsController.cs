namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
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

        public SettingsController(
            IDeletableEntityRepository<PersonRole> perRoles,
            IDeletableEntityRepository<CargoType> cargoTypes,
            IDeletableEntityRepository<VehicleLoadingBody> loadBodies)
        {
            this.perRoles = perRoles;
            this.cargoTypes = cargoTypes;
            this.loadBodies = loadBodies;
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
                return this.Json(new { isValid = false, html = this.View(input) });
            }

            var role = new PersonRole
            {
                Name = input.Name,
            };

            await this.perRoles.AddAsync(role);
            await this.perRoles.SaveChangesAsync();
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("Index", "Settings") });
        }

        public async Task<IActionResult> DeletePersonRole(int id)
        {
            var role = this.perRoles.All().FirstOrDefault(r => r.Id == id);
            if (role is not null)
            {
                this.perRoles.Delete(role);
                await this.perRoles.SaveChangesAsync();
            }

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
            var model = new SettingInputModel { Action = "AddCargoType" };
            return this.View("AddSetting", model);
        }

        [HttpPost("/CargoTypes/AddCargoType")]
        public async Task<IActionResult> AddCargoType(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View(input) });
            }

            var type = new CargoType
            {
                Name = input.Name,
            };

            await this.cargoTypes.AddAsync(type);
            await this.cargoTypes.SaveChangesAsync();
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("CargoTypes", "Settings") });
        }

        public async Task<IActionResult> DeleteCargoType(int id)
        {
            var type = this.cargoTypes.All().FirstOrDefault(r => r.Id == id);
            if (type is not null)
            {
                this.cargoTypes.Delete(type);
                await this.cargoTypes.SaveChangesAsync();
            }

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
            var model = new SettingInputModel { Action = "AddLoadingBody" };
            return this.View("AddSetting", model);
        }

        [HttpPost("/LoadingBodies/AddLoadingBody")]
        public async Task<IActionResult> AddLoadingBody(SettingInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, html = this.View(input) });
            }

            var body = new VehicleLoadingBody
            {
                Name = input.Name,
            };

            await this.loadBodies.AddAsync(body);
            await this.loadBodies.SaveChangesAsync();
            return this.Json(new { isValid = true, redirectToUrl = this.Url.Action("LoadingBodies", "Settings") });
        }

        public async Task<IActionResult> DeleteLoadingBody(int id)
        {
            var body = this.loadBodies.All().FirstOrDefault(b => b.Id == id);
            if (body is not null)
            {
                this.loadBodies.Delete(body);
                await this.loadBodies.SaveChangesAsync();
            }

            return this.RedirectToAction("LoadingBodies");
        }
    }
}
