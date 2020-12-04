namespace BCKFreightTMS.Web.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services;
    using BCKFreightTMS.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    [Authorize(Roles = "User")]
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext dbContext;

        private CompaniesManagerService companiesMan;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.companiesMan = new CompaniesManagerService();
        }

        public async Task<IActionResult> Index()
        {
            await this.companiesMan.GetJsonCompaniesAsync(@"C:\Users\mmsla\Documents\.NET projects\BCKFreight\src\Web\BCKFreightTMS.Web\wwwroot\data\20200930.json");
            var companies = this.companiesMan.Companies;
            this.ViewData["companies"] = companies;
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        //public async Task<IActionResult> SendEmail()
        //{
        //    await this.emailSender.SendEmailAsync(
        //this.Input.NewEmail,
        //"Confirm your email",
        //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
