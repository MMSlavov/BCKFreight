namespace BCKFreightTMS.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Services;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private CompaniesManagerService companiesMan;

        public DashboardController()
        {
            this.companiesMan = new CompaniesManagerService();
        }

        public async Task<IActionResult> Index()
        {
            await this.companiesMan.GetJsonCompaniesAsync(@"wwwroot\data\20200930.json");
            var companies = this.companiesMan.Companies;
            this.ViewData["companies"] = companies;
            return this.View();
        }
    }
}
