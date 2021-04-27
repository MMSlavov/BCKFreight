namespace BCKFreightTMS.Web.Areas.Administration.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IPdfService pdfService;
        private CompaniesManagerService companiesMan;

        public DashboardController(BCKFreightTMS.Services.IPdfService pdfService)
        {
            this.companiesMan = new CompaniesManagerService();
            this.pdfService = pdfService;
        }

        public async Task<IActionResult> Index()
        {
            await this.companiesMan.AddJsonCompaniesAsync(string.Format(GlobalConstants.JsonDataPath, "20200930.json"));
            var companies = this.companiesMan.Companies;
            this.ViewData["companies"] = companies;
            return this.View();
        }

        public IActionResult Scan()
        {
            var images = this.pdfService.Scan();

            using (var ms = new MemoryStream())
            {
                images[0].Save(ms, images[0].RawFormat);
                return this.File(ms.ToArray(), "image/jpeg");
            }
        }

        public IActionResult ScanOCR()
        {
            dynamic res;
            try
            {
                res = this.pdfService.ScanTessOCR().ToArray();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return this.Json(res);
        }

        [HttpPost]
        public async Task<IActionResult> FileOCR(IFormFile file)
        {
            string res;
            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    res = this.pdfService.FileTessOCR(ms.ToArray());
                }
            }
            catch (System.Exception ex)
            {
                res = ex.Message;
            }

            return this.Json(res);
        }

        public IActionResult Sandbox()
        {
            return this.View();
        }
    }
}
