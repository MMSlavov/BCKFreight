namespace BCKFreightTMS.Services.Data.Tests
{
    using System.Linq;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Data.Repositories;
    using BCKFreightTMS.Web.ViewModels.Settings;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class SettingsServiceTests
    {
        private EfDeletableEntityRepository<PersonRole> personRolesRepo;
        private EfDeletableEntityRepository<CargoType> cargoTypesRepo;
        private EfDeletableEntityRepository<VehicleLoadingBody> loadBodiesRepo;

        [Fact]
        public async void AddPersonRoleTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetSettingsService(dbContext);
            var role = new SettingInputModel { Name = "test1" };
            await service.AddPersonRoleAsync(role);
            Assert.True(dbContext.PersonRoles.Any(r => r.Name == role.Name));
            Assert.Equal(2, dbContext.PersonRoles.Count());
        }

        [Fact]
        public async void DeletePersonRoleTest()
        {
            var dbContext = this.GetDbContext();

            var service = this.GetSettingsService(dbContext);

            var id = this.personRolesRepo.AllAsNoTracking().First().Id;
            await service.DeletePersonRoleAsync(id);
            var role = this.personRolesRepo.AllWithDeleted().FirstOrDefault(r => r.Id == id);
            Assert.True(role.IsDeleted);
            Assert.Equal(0, dbContext.PersonRoles.Count());
        }

        [Fact]
        public async void AddCargoTypeTest()
        {
            using var dbContext = this.GetDbContext();
            var role = new SettingInputModel { Name = "test1" };
            var service = this.GetSettingsService(dbContext);
            await service.AddCargoTypeAsync(role);
            Assert.True(dbContext.CargoTypes.Any(r => r.Name == role.Name));
            Assert.Equal(2, dbContext.CargoTypes.Count());
        }

        [Fact]
        public async void DeleteCargoTypeTest()
        {
            var dbContext = this.GetDbContext();

            var service = this.GetSettingsService(dbContext);

            var id = this.cargoTypesRepo.AllAsNoTracking().First().Id;
            await service.DeleteCargoTypeAsync(id);
            var type = this.cargoTypesRepo.AllWithDeleted().FirstOrDefault(r => r.Id == id);
            Assert.True(type.IsDeleted);
            Assert.Equal(0, dbContext.CargoTypes.Count());
        }

        [Fact]
        public async void AddLoadingBodyTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetSettingsService(dbContext);
            var body = new SettingInputModel { Name = "test" };
            await service.AddLoadingBodyAsync(body);
            Assert.True(dbContext.VehicleLoadingBodies.Any(r => r.Name == body.Name));
            Assert.Equal(2, dbContext.VehicleLoadingBodies.Count());
        }

        [Fact]
        public async void DeleteLoadingBodyTest()
        {
            var dbContext = this.GetDbContext();

            var service = this.GetSettingsService(dbContext);

            var id = this.loadBodiesRepo.AllAsNoTracking().First().Id;
            await service.DeleteLoadingBodyAsync(id);
            var role = this.loadBodiesRepo.AllWithDeleted().FirstOrDefault(r => r.Id == id);
            Assert.True(role.IsDeleted);
            Assert.Equal(0, dbContext.VehicleLoadingBodies.Count());
        }

        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.PersonRoles.Add(new PersonRole { Name = PersonRoleNames.Employee.ToString() });
            dbContext.CargoTypes.Add(new CargoType { Name = "Pallet" });
            dbContext.VehicleLoadingBodies.Add(new VehicleLoadingBody { Name = LoadingBodyNames.Tarpaulin.ToString() });
            dbContext.SaveChanges();
            return dbContext;
        }

        private SettingsService GetSettingsService(ApplicationDbContext dbContext)
        {
            var repoFactory = new RepositoryFactory();
            this.personRolesRepo = repoFactory.GetEfDeletableEntityRepository<PersonRole>(dbContext);
            this.cargoTypesRepo = repoFactory.GetEfDeletableEntityRepository<CargoType>(dbContext);
            this.loadBodiesRepo = repoFactory.GetEfDeletableEntityRepository<VehicleLoadingBody>(dbContext);

            return new SettingsService(this.personRolesRepo, this.cargoTypesRepo, this.loadBodiesRepo);
        }
    }
}
