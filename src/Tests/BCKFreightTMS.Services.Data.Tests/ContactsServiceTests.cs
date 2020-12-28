namespace BCKFreightTMS.Services.Data.Tests
{
    using System.Linq;

    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ContactsServiceTests
    {
        private IDeletableEntityRepository<Company> companiesRepository;
        private IDeletableEntityRepository<Person> peopleRepository;
        private IDeletableEntityRepository<TaxCountry> taxCtrRepo;
        private IDeletableEntityRepository<PersonRole> personRolesRepo;

        [Fact]
        public async void AddCompanyTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel { Name = "test" };
            var res = await service.AddCompanyAsync(company);
            Assert.NotNull(res);
            Assert.True(dbContext.Companies.Any(x => x.Name == company.Name));
        }

        [Fact]
        public async void AddingSameCompanyTwiceShouldReturnNull()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel { Name = "test" };
            await service.AddCompanyAsync(company);
            var res = await service.AddCompanyAsync(company);
            Assert.Null(res);
        }

        [Fact]
        public void GetPersonInputModelTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.Companies.Add(new Company { Name = "test1" });
            dbContext.Companies.Add(new Company { Name = "test2" });
            dbContext.PersonRoles.Add(new PersonRole { Name = "test1" });
            dbContext.PersonRoles.Add(new PersonRole { Name = "test2" });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var model = service.GetPersonInputModel();
            Assert.NotNull(model);
            Assert.Equal(2, model.CompanyItems.Count());
            Assert.Equal(2, model.RoleItems.Count());
        }

        [Fact]
        public async void AddPersonTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var person = new PersonInputModel { FirstName = "testFN", LastName = "testLN" };
            var res = await service.AddPersonAsync(person);
            Assert.NotNull(res);
            Assert.True(dbContext.People.Any(x => x.FirstName == person.FirstName));
        }

        [Fact]
        public async void AddingSamePersonTwiceShouldReturnNull()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var person = new PersonInputModel { FirstName = "testFN", LastName = "testLN" };
            await service.AddPersonAsync(person);
            var res = await service.AddPersonAsync(person);
            Assert.Null(res);
        }

        [Fact]
        public void GetAllTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.People.Add(new Person { FirstName = "testFN", LastName = "testLN" });
            dbContext.Companies.Add(new Company { Name = "testC" });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetAll();
            Assert.Equal(2, res.Count());
        }

        [Fact]
        public void GetPersonDetailsTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.People.Add(new Person { FirstName = "testFN", LastName = "testLN" });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetContactDetails(dbContext.People.First().Id);
            Assert.Equal(3, res.Count());
        }

        [Fact]
        public void GetCompanyDetailsTest()
        {
            using var dbContext = this.GetDbContext();
            dbContext.Companies.Add(new Company { Name = "testFN" });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetContactDetails(dbContext.Companies.First().Id);
            Assert.Equal(2, res.Count());
        }

        [Fact]
        public void GetDetailsInvalidIdSouldReturnNull()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var res = service.GetContactDetails(string.Empty);
            Assert.Null(res);
        }

        [Fact]
        public async void DeletePersonTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var person = new PersonInputModel { FirstName = "testFN", LastName = "testLN" };
            await service.AddPersonAsync(person);
            var res = await service.DeleteAsync(dbContext.People.First().Id);
            Assert.True(res);
            Assert.Equal(0, dbContext.People.Count());
        }

        [Fact]
        public async void DeleteCompanyTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel { Name = "testC" };
            await service.AddCompanyAsync(company);
            var res = await service.DeleteAsync(dbContext.Companies.First().Id);
            Assert.True(res);
            Assert.Equal(0, dbContext.Companies.Count());
        }

        [Fact]
        public async void Test()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel { Name = "testC" };
            await service.AddCompanyAsync(company);
            var res = await service.DeleteAsync(dbContext.Companies.First().Id);
            Assert.True(res);
            Assert.Equal(0, dbContext.Companies.Count());
        }

        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            return dbContext;
        }

        private ContactsService GetContactsService(ApplicationDbContext dbContext)
        {
            var repoFactory = new RepositoryFactory();
            this.companiesRepository = repoFactory.GetEfDeletableEntityRepository<Company>(dbContext);
            this.peopleRepository = repoFactory.GetEfDeletableEntityRepository<Person>(dbContext);
            this.taxCtrRepo = repoFactory.GetEfDeletableEntityRepository<TaxCountry>(dbContext);
            this.personRolesRepo = repoFactory.GetEfDeletableEntityRepository<PersonRole>(dbContext);

            return new ContactsService(this.companiesRepository, this.peopleRepository, this.taxCtrRepo, this.personRolesRepo);
        }
    }
}
