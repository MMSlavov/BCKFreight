namespace BCKFreightTMS.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Settings;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<PersonRole> perRoles;
        private readonly IDeletableEntityRepository<CargoType> cargoTypes;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadBodies;

        public SettingsService(
            IDeletableEntityRepository<PersonRole> perRoles,
            IDeletableEntityRepository<CargoType> cargoTypes,
            IDeletableEntityRepository<VehicleLoadingBody> loadBodies)
        {
            this.perRoles = perRoles;
            this.cargoTypes = cargoTypes;
            this.loadBodies = loadBodies;
        }

        public async Task AddPersonRoleAsync(SettingInputModel input)
        {
            var role = new PersonRole
            {
                Name = input.Name,
            };

            await this.perRoles.AddAsync(role);
            await this.perRoles.SaveChangesAsync();
        }

        public async Task DeletePersonRoleAsync(int id)
        {
            var role = this.perRoles.All().FirstOrDefault(r => r.Id == id);
            if (role is not null)
            {
                this.perRoles.Delete(role);
                await this.perRoles.SaveChangesAsync();
            }
        }

        public async Task AddCargoTypeAsync(SettingInputModel input)
        {
            var type = new CargoType
            {
                Name = input.Name,
            };

            await this.cargoTypes.AddAsync(type);
            await this.cargoTypes.SaveChangesAsync();
        }

        public async Task DeleteCargoTypeAsync(int id)
        {
            var type = this.cargoTypes.All().FirstOrDefault(r => r.Id == id);
            if (type is not null)
            {
                this.cargoTypes.Delete(type);
                await this.cargoTypes.SaveChangesAsync();
            }
        }

        public async Task AddLoadingBodyAsync(SettingInputModel input)
        {
            var body = new VehicleLoadingBody
            {
                Name = input.Name,
            };

            await this.loadBodies.AddAsync(body);
            await this.loadBodies.SaveChangesAsync();
        }

        public async Task DeleteLoadingBodyAsync(int id)
        {
            var body = this.loadBodies.All().FirstOrDefault(b => b.Id == id);
            if (body is not null)
            {
                this.loadBodies.Delete(body);
                await this.loadBodies.SaveChangesAsync();
            }
        }
    }
}
