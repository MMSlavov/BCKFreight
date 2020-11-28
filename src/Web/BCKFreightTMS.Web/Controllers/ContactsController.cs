namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Linq;
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

        public async Task<IActionResult> AddCompany(string uic = null)
        {
            try
            {
                var model = uic != null ? await this.companiesManager.GetCompanyAsync(int.Parse(uic)) : null;
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
                return this.View(input);
            }

            var res = await this.contactsService.AddPersonAsync(input);
            if (res == null)
            {
                this.ModelState.AddModelError(string.Empty, "Person allready exists.");
                return this.View(input);
            }

            return this.Redirect("/Contacts");
        }

        public async Task<IActionResult> Delete(string contactType, string id)
        {
            if (contactType == nameof(Company))
            {
                var company = this.companies.All().FirstOrDefault(c => c.Id == id);
                this.companies.Delete(company);
                await this.companies.SaveChangesAsync();
            }
            else if (contactType == nameof(Person))
            {
                var person = this.people.All().FirstOrDefault(p => p.Id == id);
                this.people.Delete(person);
                await this.people.SaveChangesAsync();
            }

            return this.Redirect("/Contacts");
        }

        public IActionResult CompanyDetails(string? id)
        {
            var company = this.companies.All().Where(c => c.Id == id).To<CompanyViewModel>().FirstOrDefault();
            return this.View(company);
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
