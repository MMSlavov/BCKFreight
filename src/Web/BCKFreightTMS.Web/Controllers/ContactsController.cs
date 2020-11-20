namespace BCKFreightTMS.Web.Controllers
{
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ContactsController : Controller
    {
        private readonly IContactsService contactsService;

        public ContactsController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
        }

        public IActionResult All()
        {
            var contacts = this.contactsService.GetAll();
            return this.View(contacts);
        }

        public IActionResult CreateCompany()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.contactsService.AddCompanyAsync(model);

            return this.Redirect("/Contacts/All");
        }
    }
}
