namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class ContactsController : Controller
    {
        private readonly IContactsService contactsService;
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly IDeletableEntityRepository<Person> people;
        private readonly IDeletableEntityRepository<PersonRole> roles;
        private readonly ICompaniesManagerService companiesManager;

        public ContactsController(
            IContactsService contactsService,
            IDeletableEntityRepository<Company> compRepo,
            IDeletableEntityRepository<Person> peopRepo,
            IDeletableEntityRepository<PersonRole> rolesRepo,
            ICompaniesManagerService companiesManager)
        {
            this.contactsService = contactsService;
            this.companies = compRepo;
            this.people = peopRepo;
            this.roles = rolesRepo;
            this.companiesManager = companiesManager;
        }

        public IActionResult Index()
        {
            var contacts = this.contactsService.GetAll();
            return this.View(contacts);
        }

        [HttpPost]
        public IActionResult GetContacts()
        {
            try
            {
                return this.Ok(this.contactsService.ProcessDataTableRequest(this.Request));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> AddCompany(string searchStr = null)
        {
            try
            {
                var model = searchStr != null ? await this.companiesManager.GetCompanyAsync(searchStr) : null;
                return this.View(model);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View();
            }
        }

        public IActionResult AddPerson()
        {
            var viewModel = this.contactsService.GetPersonInputModel();
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson(PersonInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(this.contactsService.GetPersonInputModel(input));
            }

            var res = await this.contactsService.AddPersonAsync(input);
            if (res == null)
            {
                this.ModelState.AddModelError(string.Empty, "Person allready exists.");
                return this.View(input);
            }

            return this.Redirect("/Contacts");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (this.companies.AllAsNoTracking().Any(c => c.Id == id))
            {
                var company = this.companies.All().FirstOrDefault(c => c.Id == id);
                this.companies.Delete(company);
                await this.companies.SaveChangesAsync();
            }
            else if (this.people.AllAsNoTracking().Any(p => p.Id == id))
            {
                var person = this.people.All().FirstOrDefault(p => p.Id == id);
                this.people.Delete(person);
                await this.people.SaveChangesAsync();
            }

            return this.Redirect("/Contacts");
        }

        public IActionResult Details(string id)
        {
            var data = new Dictionary<string, string>();
            if (this.companies.AllAsNoTracking().Any(c => c.Id == id))
            {
                var company = this.companies.All().FirstOrDefault(c => c.Id == id);
                data.Add("Id", company.Id);
                data.Add("Name", company.Name);
                data.Add("Tax country", company.TaxCountry.Name);
                data.Add("Tax number", company.TaxNumber);
                data.Add("Address", company.Address.Address.StreetLine);
                data.Add("Mobile", company.Comunicators.Mobile1);
                data.Add("Details", company.Comunicators.Details);
            }
            else if (this.people.AllAsNoTracking().Any(p => p.Id == id))
            {
                var person = this.people.All().FirstOrDefault(p => p.Id == id);
                data.Add("Id", person.Id);
                data.Add("First name", person.FirstName);
                data.Add("Last name", person.LastName);
                data.Add("Birthday", person.BirthDate.ToLocalTime().ToShortDateString());
                data.Add("Mobile", person.Comunicators.Mobile1);
            }

            return this.View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var res = await this.contactsService.AddCompanyAsync(model);
            if (res == null)
            {
                this.ModelState.AddModelError(string.Empty, "Company allready exists.");
                return this.View(model);
            }

            return this.Redirect("/Contacts");
        }
    }
}
