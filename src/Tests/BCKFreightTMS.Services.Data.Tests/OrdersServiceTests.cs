namespace BCKFreightTMS.Services.Data.Tests
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class OrdersServiceTests
    {
        private IDeletableEntityRepository<Company> companies;
        private IDeletableEntityRepository<Order> orders;
        private IDeletableEntityRepository<VehicleLoadingBody> loadingBodies;
        private IDeletableEntityRepository<CargoType> cargoTypes;
        private IDeletableEntityRepository<Person> people;
        private IDeletableEntityRepository<Vehicle> vehicles;
        private IDeletableEntityRepository<ActionNotFinishedReason> actionNFReasons;
        private IDeletableEntityRepository<OrderAction> orderActions;
        private IDeletableEntityRepository<ActionType> actionTypes;
        private IDeletableEntityRepository<OrderStatus> orderStatuses;
        private IDeletableEntityRepository<VehicleType> vehicleTypes;

        [Fact]
        public void GetAllTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var res = service.GetAll<ListOrderViewModel>();
            Assert.NotNull(res);
        }

        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            return dbContext;
        }

        private OrdersService GetContactsService(ApplicationDbContext dbContext)
        {
            var repoFactory = new RepositoryFactory();
            this.companies = repoFactory.GetEfDeletableEntityRepository<Company>(dbContext);
            this.orders = repoFactory.GetEfDeletableEntityRepository<Order>(dbContext);
            this.loadingBodies = repoFactory.GetEfDeletableEntityRepository<VehicleLoadingBody>(dbContext);
            this.cargoTypes = repoFactory.GetEfDeletableEntityRepository<CargoType>(dbContext);
            this.people = repoFactory.GetEfDeletableEntityRepository<Person>(dbContext);
            this.actionNFReasons = repoFactory.GetEfDeletableEntityRepository<ActionNotFinishedReason>(dbContext);
            this.orderActions = repoFactory.GetEfDeletableEntityRepository<OrderAction>(dbContext);
            this.actionTypes = repoFactory.GetEfDeletableEntityRepository<ActionType>(dbContext);
            this.orderStatuses = repoFactory.GetEfDeletableEntityRepository<OrderStatus>(dbContext);
            this.vehicleTypes = repoFactory.GetEfDeletableEntityRepository<VehicleType>(dbContext);
            this.vehicles = repoFactory.GetEfDeletableEntityRepository<Vehicle>(dbContext);
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userMan = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userMan.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { AdminId = string.Empty }));
            return new OrdersService(
                this.companies,
                this.orders,
                this.loadingBodies,
                this.cargoTypes,
                this.people,
                this.vehicles,
                userMan.Object,
                this.actionNFReasons,
                this.orderActions,
                this.actionTypes,
                this.orderStatuses,
                this.vehicleTypes);
        }
    }