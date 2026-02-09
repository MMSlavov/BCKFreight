namespace BCKFreightTMS.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Messaging;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ContactsServiceTests
    {
        [Fact]
        public async Task AddCompanyTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel
            {
                Name = "test",
                TaxCountry = "Bulgaria",
                Mobile1 = "123",
            };
            var res = await service.AddCompanyAsync(company);

            Assert.NotNull(res);
            Assert.True(dbContext.Companies.Any(x => x.Name == company.Name));
        }

        [Fact]
        public async Task AddingSameCompanyTwiceShouldReturnNull()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel
            {
                Name = "test",
                TaxCountry = "Bulgaria",
                Mobile1 = "123",
            };
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
        public async Task AddPersonTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var person = new PersonInputModel { FirstName = "testFN", LastName = "testLN" };
            var res = await service.AddPersonAsync(person);

            Assert.NotNull(res);
            Assert.True(dbContext.People.Any(x => x.FirstName == person.FirstName));
        }

        [Fact]
        public async Task AddingSamePersonTwiceShouldThrowException()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var person = new PersonInputModel { FirstName = "testFN", LastName = "testLN" };
            await service.AddPersonAsync(person);

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddPersonAsync(person));
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
            var company = dbContext.Companies.Add(new Company
            {
                Name = "testFN",
                TaxCountry = null,
            });
            dbContext.SaveChanges();
            var service = this.GetContactsService(dbContext);
            var res = service.GetContactDetails(company.Entity.Id);

            Assert.Equal(3, res.Count());
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
        public async Task DeletePersonTest()
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
        public async Task DeleteCompanyTest()
        {
            using var dbContext = this.GetDbContext();
            var service = this.GetContactsService(dbContext);
            var company = new CompanyInputModel
            {
                Name = "testC",
                TaxCountry = "Bulgaria",
            };
            await service.AddCompanyAsync(company);
            var res = await service.DeleteAsync(dbContext.Companies.First().Id);

            Assert.True(res);
            Assert.Equal(0, dbContext.Companies.Count());
        }

        private ApplicationDbContext GetDbContext()
        {
            var uniqueDatabaseName = $"ContactsServiceTestDb_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: uniqueDatabaseName).Options;
            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        private ContactsService GetContactsService(ApplicationDbContext dbContext)
        {
            var repoFactory = new RepositoryFactory();
            var companiesRepository = repoFactory.GetEfDeletableEntityRepository<Company>(dbContext);
            var peopleRepository = repoFactory.GetEfDeletableEntityRepository<Person>(dbContext);
            var taxCtrRepo = repoFactory.GetEfDeletableEntityRepository<TaxCountry>(dbContext);
            var personRolesRepo = repoFactory.GetEfDeletableEntityRepository<PersonRole>(dbContext);
            var bankDetails = repoFactory.GetEfDeletableEntityRepository<BankDetails>(dbContext);
            var emailSender = new Mock<IEmailSender>();
            var mapper = new Mock<IMapper>();

            return new ContactsService(companiesRepository, peopleRepository, taxCtrRepo, personRolesRepo, bankDetails, mapper.Object, emailSender.Object);
        }
    }
}
