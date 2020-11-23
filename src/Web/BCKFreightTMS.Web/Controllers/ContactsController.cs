namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : Controller
    {
        private readonly IContactsService contactsService;
        private readonly IDeletableEntityRepository<Company> companies;

        public ContactsController(IContactsService contactsService, IDeletableEntityRepository<Company> compRepo)
        {
            this.contactsService = contactsService;
            this.companies = compRepo;
        }

        public IActionResult Index()
        {
            var contacts = this.contactsService.GetAll();
            return this.View(contacts);
        }

        public IActionResult CreateCompany()
        {
            return this.View();
        }

        public IActionResult AddPerson()
        {
            return this.View();
        }

        public IActionResult CompanyDetails(string? id)
        {
            var company = this.companies.All().FirstOrDefault(c => c.Id == id);
            return this.View(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.contactsService.AddCompanyAsync(model);

            return this.Redirect("/Contacts");
        }
    }
}
