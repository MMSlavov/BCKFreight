namespace BCKFreightTMS.Services.Data.Tests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Data.Repositories;
    using BCKFreightTMS.Services.Messaging;
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
        private EfDeletableEntityRepository<Currency> currencies;
        private EfDeletableEntityRepository<Documentation> documentations;
        private IDeletableEntityRepository<ActionNotFinishedReason> actionNFReasons;
        private IDeletableEntityRepository<OrderAction> orderActions;
        private IDeletableEntityRepository<ActionType> actionTypes;
        private IDeletableEntityRepository<OrderStatus> orderStatuses;
        private IDeletableEntityRepository<VehicleType> vehicleTypes;

        // [Fact]
        // public void GetAllTest()
        // {
        //    using var dbContext = this.GetDbContext();
        //    var service = this.GetContactsService(dbContext);

        // var res = service.GetAll<ListOrderViewModel>();
        //    Assert.NotNull(res);
        // }
        // [Fact]
        // public void GetAllNullTest()
        // {
        //    using var dbContext = this.GetDbContext();
        //    var service = this.GetContactsService(dbContext);
        //    var res = service.GetAll<ListOrderViewModel>().ToList();
        //    Assert.Empty(res);
        // }
        [Fact]
        public void LoadOrderInputModelTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.Orders.Add(new Order { CreatorId = "test", CargoId = "test" });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.LoadOrderAcceptInputModel();
            Assert.NotNull(res);
        }

        [Fact]
        public void GetContactsTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.People.Add(new Person
            {
                FirstName = "testFN",
                LastName = "testLN",
                CompanyId = "testId",
                Role = new PersonRole { Name = "Contact" },
            });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetContacts("testId");
            Assert.NotNull(res);
            Assert.Equal("testFN testLN", res.First().Text);
        }

        [Fact]
        public void GetDriversTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.People.Add(new Person
            {
                FirstName = "testFN",
                LastName = "testLN",
                CompanyId = "testId",
                Role = new PersonRole { Name = "Driver" },
            });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetDrivers("testId");
            Assert.NotNull(res);
            Assert.Equal("testFN testLN", res.First().Text);
        }

        [Fact]
        public void GetVehiclesTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.Vehicles.Add(new Vehicle
            {
                TypeId = 0,
                CompanyId = "testId",
                RegNumber = "testRN",
            });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetVehicles("testId");
            Assert.NotNull(res);
            Assert.Equal("testRN", res.First().Text);
        }

        [Fact]
        public async void CreateOrderTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.VehicleTypes.Add(new VehicleType { Name = "Truck" });
            dbContext.OrderStatuses.Add(new OrderStatus { Name = "InProgress" });
            dbContext.ActionTypes.AddRange(new ActionType { Name = "Loading" }, new ActionType { Name = "Unloading" });

            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var model = new OrderAcceptInputModel
            {
                CompanyFromId = "test",

                // CompanyToId = "test",
                // DriverId = "test",
            };
            var res = await service.AcceptAsync(model, new ClaimsPrincipal());
            Assert.NotNull(res);
        }

        [Fact]
        public async void DeleteOrderTest()
        {
            using var dbContext = this.GetDbContext();
            var order = new Order { CreatorId = "test", CargoId = "test" };
            order.OrderActions.Add(new OrderAction { OrderId = order.Id });
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = await service.DeleteAsync(dbContext.Orders.First().Id);
            Assert.True(res);
        }

        [Fact]
        public async void DeleteOrderNullTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var res = await service.DeleteAsync(string.Empty);
            Assert.False(res);
        }

        // [Fact]
        // public async void LoadOrderStatusModelTest()
        // {
        //    using var dbContext = this.GetDbContext();
        //    dbContext.VehicleTypes.Add(new VehicleType { Name = "Truck" });
        //    dbContext.OrderStatuses.Add(new OrderStatus { Name = "InProgress" });
        //    dbContext.ActionTypes.AddRange(new ActionType { Name = "Loading" }, new ActionType { Name = "Unloading" });

        // dbContext.SaveChanges();
        //    var service = this.GetContactsService(dbContext);
        //    var model = new OrderInputModel
        //    {
        //        CompanyFromId = "test",
        //        CompanyToId = "test",
        //        DriverId = "test",
        //    };
        //    await service.CreateAsync(model, new ClaimsPrincipal());
        //    var res = service.LoadOrderStatusModel(dbContext.Orders.First().Id);
        //    Assert.NotNull(res);
        // }
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
            this.currencies = repoFactory.GetEfDeletableEntityRepository<Currency>(dbContext);
            this.documentations = repoFactory.GetEfDeletableEntityRepository<Documentation>(dbContext);

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userMan = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userMan.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { AdminId = string.Empty }));
            var mapper = new Mock<IMapper>();
            var emailSender = new Mock<IEmailSender>();

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
                this.vehicleTypes,
                this.currencies,
                this.documentations,
                mapper.Object,
                emailSender.Object);
        }
    }
}
