namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BCKFreightTMS.Services;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class ContactsController : Controller
    {
        private readonly IContactsService contactsService;
        private readonly ICompaniesManagerService companiesManager;

        public ContactsController(
            IContactsService contactsService,
            ICompaniesManagerService companiesManager)
        {
            this.contactsService = contactsService;
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
            await this.contactsService.DeleteAsync(id);

            return this.RedirectToAction("Contacts");
        }

        public IActionResult Details(string id)
        {
            var data = this.contactsService.GetContactDetails(id);

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
