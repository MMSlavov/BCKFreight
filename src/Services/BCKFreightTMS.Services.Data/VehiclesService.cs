namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Vehicles;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class VehiclesService : IVehiclesService
    {
        private readonly IDeletableEntityRepository<Vehicle> vehicles;
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly IDeletableEntityRepository<VehicleType> types;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadingBodies;
        private readonly IDeletableEntityRepository<Person> people;

        public VehiclesService(
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

        public IEnumerable<T> GetAll<T>()
        {
            var vehicles = this.vehicles.All().To<T>().ToList();
            return vehicles;
        }

        public VehicleInputModel LoadVehicleInputModel(VehicleInputModel model = null)
        {
            if (model is null)
            {
                model = new VehicleInputModel();
            }

            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new System.Collections.Generic.KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                               .ToList();
            model.TypeItems = this.types.AllAsNoTracking()
                                       .Select(t => new System.Collections.Generic.KeyValuePair<string, string>(t.Id.ToString(), t.Name))
                                       .ToList();
            model.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                       .Select(lb => new System.Collections.Generic.KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                                       .ToList();
            return model;
        }

        public IEnumerable<SelectListItem> GetDrivers(string companyId)
        {
            var drivers = this.people.AllAsNoTracking()
                                     .Where(p => p.CompanyId == companyId && p.Role.Name == PersonRoleNames.Driver.ToString())
                                     .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                                     .ToList();
            return drivers;
        }

        public IEnumerable<SelectListItem> GetTrailers(string companyId)
        {
            var trailers = this.vehicles.AllAsNoTracking()
                                     .Where(v => v.CompanyId == companyId && v.Type.Name == VehicleTypeNames.Trailer.ToString())
                                     .Select(t => new SelectListItem { Text = t.RegNumber, Value = t.Id })
                                     .ToList();
            return trailers;
        }

        public async Task<string> AddVehicleAsync(VehicleInputModel input)
        {
            if (this.vehicles.AllAsNoTracking().Any(v => v.RegNumber == input.RegNumber))
            {
                throw new ArgumentException("Vehicle already exist.");
            }

            var vehicle = new Vehicle
            {
                TypeId = input.TypeId,
                LoadingBodyId = input.LoadingBodyId == 0 ? null : input.LoadingBodyId,
                CompanyId = input.CompanyId,
                DriverId = input.DriverId == "null" ? null : input.DriverId,
                TrailerId = input.TrailerId == "null" ? null : input.TrailerId,
                RegNumber = input.RegNumber,
                Name = input.Name,
                Details = input.Details,
            };

            await this.vehicles.AddAsync(vehicle);
            await this.vehicles.SaveChangesAsync();

            return vehicle.Id;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var model = this.vehicles.All().FirstOrDefault(v => v.Id == id);
            if (model is null)
            {
                return false;
            }

            this.vehicles.Delete(model);
            return await this.vehicles.SaveChangesAsync() > 0;
        }
    }
}
