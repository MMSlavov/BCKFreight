namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
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
        private readonly IDeletableEntityRepository<PersonRole> personRoles;
        private readonly INotyfService notyfService;

        public ContactsController(
            IContactsService contactsService,
            ICompaniesManagerService companiesManager,
            IDeletableEntityRepository<PersonRole> personRoles,
            INotyfService notyfService)
        {
            this.contactsService = contactsService;
            this.companiesManager = companiesManager;
            this.personRoles = personRoles;
            this.notyfService = notyfService;
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

        public IActionResult AddCompany()
        {
            return this.View();
        }

        public IActionResult AddCompanyModal()
        {
            return this.View();
        }

        public IActionResult EditCompanyModal(string id)
        {
            var model = this.contactsService.LoadEditCompanyModel(id);

            return this.View(model);
        }

        public async Task<IActionResult> GetCompany(string searchStr = null)
        {
            try
            {
                var model = searchStr != null ? await this.companiesManager.GetCompanyAsync(searchStr) : null;
                return this.Json(model);
            }
            catch (InvalidOperationException ex)
            {
                this.notyfService.Error(ex.Message);
                return this.Json(new { });
            }
        }

        // public async Task<IActionResult> GetCompanySN()
        // {
        //    try
        //    {
        //        var html = await this.companiesManager.SpeditorNetGetCompanyAsync("");
        //        var model = new CompanySNModel { Html = html };
        //        return this.View(model);
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        return this.Problem();
        //    }
        // }
        public IActionResult AddPerson()
        {
            var viewModel = this.contactsService.GetPersonInputModel();
            return this.View(viewModel);
        }

        public IActionResult AddPersonModal(string id, string role)
        {
            var viewModel = this.contactsService.GetPersonInputModel();
            var roleId = this.personRoles.AllAsNoTracking().FirstOrDefault(r => r.Name == role).Id;
            viewModel.CompanyId = id;
            viewModel.RoleId = roleId;
            viewModel.BirthDate = null;
            return this.View(viewModel);
        }

        public IActionResult AddBankDetailsModal(string id)
        {
            var viewModel = new BankDetailsModel();
            viewModel.CompanyId = id;
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson(PersonInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(this.contactsService.GetPersonInputModel(input));
            }

            try
            {
                await this.contactsService.AddPersonAsync(input);
            }
            catch (Exception ex)
            {
                this.notyfService.Error(ex.Message);
            }

            return this.Redirect(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonModal(PersonInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(this.contactsService.GetPersonInputModel(input));
            }

            try
            {
                await this.contactsService.AddPersonAsync(input);
            }
            catch (Exception ex)
            {
                this.notyfService.Error(ex.Message);
            }

            return this.Json(new { isValid = true, redirectToUrl = string.Empty });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.contactsService.DeleteAsync(id);

            return this.RedirectToAction(GlobalConstants.Index);
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

            return this.Redirect(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyModal(CompanyInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, redirectToUrl = string.Empty, html = this.View(model) });
            }

            var res = await this.contactsService.AddCompanyAsync(model);
            if (res == null)
            {
                this.ModelState.AddModelError(string.Empty, "Company allready exists.");
                return this.Json(new { isValid = false, redirectToUrl = string.Empty, html = this.View(model) });
            }

            return this.Json(new { isValid = true, redirectToUrl = "reload" });
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyModal(CompanyEditModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, redirectToUrl = string.Empty, html = this.View(model) });
            }

            try
            {
                await this.contactsService.EditCompanyAsync(model);
            }
            catch (Exception)
            {
                return this.Json(new { isValid = false, redirectToUrl = "reload" });
            }

            return this.Json(new { isValid = true, redirectToUrl = "reload" });
        }

        [HttpPost]
        public async Task<IActionResult> AddBankDetailsModal(BankDetailsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Json(new { isValid = false, redirectToUrl = string.Empty, html = this.View(model) });
            }

            await this.contactsService.AddBankDetails(model);

            return this.Json(new { isValid = true, redirectToUrl = string.Empty });
        }

        public JsonResult GetBankDetails(string companyId)
        {
            var bankDetails = this.contactsService.GetBankDetails(companyId);
            return this.Json(bankDetails);
        }
    }
}
