namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Vehicles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize(Roles = "User")]
    public class VehiclesController : Controller
    {
        private readonly IDeletableEntityRepository<Vehicle> vehicles;
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly IDeletableEntityRepository<VehicleType> types;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadingBodies;
        private readonly IDeletableEntityRepository<Person> people;

        public VehiclesController(
            IDeletableEntityRepository<Vehicle> vehicles,
            IDeletableEntityRepository<Company> companies,
            IDeletableEntityRepository<VehicleType> types,
            IDeletableEntityRepository<VehicleLoadingBody> loadingBodies,
            IDeletableEntityRepository<Person> people)
        {
            this.vehicles = vehicles;
            this.companies = companies;
            this.types = types;
            this.loadingBodies = loadingBodies;
            this.people = people;
        }

        public IActionResult Index()
        {
            var vehicles = this.vehicles.All().To<VehicleListViewModel>().ToList();
            return this.View(vehicles);
        }

        public IActionResult AddVehicle()
        {
            var model = new VehicleInputModel();
            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new System.Collections.Generic.KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                               .ToList();
            model.TypeItems = this.types.AllAsNoTracking()
                                   .Select(t => new System.Collections.Generic.KeyValuePair<string, string>(t.Id.ToString(), t.Name))
                                   .ToList();
            model.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                   .Select(lb => new System.Collections.Generic.KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                   .ToList();
            return this.View(model);
        }

        public JsonResult GetDrivers(string companyId)
        {
            var drivers = this.people.AllAsNoTracking()
                                     .Where(p => p.CompanyId == companyId && p.Role.Name == PersonRoleNames.Driver.ToString())
                                     .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                                     .ToList();
            return this.Json(drivers);
        }

        public JsonResult GetTrailers(string companyId)
        {
            var trailers = this.vehicles.AllAsNoTracking()
                                     .Where(v => v.CompanyId == companyId && v.Type.Name == VehicleTypeNames.Trailer.ToString())
                                     .Select(t => new SelectListItem { Text = t.RegNumber, Value = t.Id })
                                     .ToList();
            return this.Json(trailers);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CompanyItems = this.companies.AllAsNoTracking()
                                   .Select(c => new System.Collections.Generic.KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                   .ToList();
                input.TypeItems = this.types.AllAsNoTracking()
                                       .Select(t => new System.Collections.Generic.KeyValuePair<string, string>(t.Id.ToString(), t.Name))
                                       .ToList();
                input.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                       .Select(lb => new System.Collections.Generic.KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                       .ToList();
                return this.View(input);
            }

            var vehicle = new Vehicle
            {
                TypeId = input.TypeId,
                LoadingBodyId = input.LoadingBodyId,
                CompanyId = input.CompanyId,
                DriverId = input.DriverId == "null" ? null : input.DriverId,
                TrailerId = input.TrailerId == "null" ? null : input.TrailerId,
                RegNumber = input.RegNumber,
                Name = input.Name,
                Details = input.Details,
            };

            await this.vehicles.AddAsync(vehicle);
            await this.vehicles.SaveChangesAsync();

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var model = this.vehicles.All().FirstOrDefault(v => v.Id == id);
            if (model is null)
            {
                return this.RedirectToAction(GlobalConstants.Index);
            }

            this.vehicles.Delete(model);
            await this.vehicles.SaveChangesAsync();
            return this.RedirectToAction(GlobalConstants.Index);
        }
    }
}
