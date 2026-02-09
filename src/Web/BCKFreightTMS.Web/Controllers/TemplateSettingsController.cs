namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Settings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class TemplateSettingsController : Controller
    {
        private readonly IApplicationTemplateService templateService;
        private readonly ITemplatePlaceholderService placeholderService;

        public TemplateSettingsController(
            IApplicationTemplateService templateService,
            ITemplatePlaceholderService placeholderService)
        {
            this.templateService = templateService;
            this.placeholderService = placeholderService;
        }

        public async Task<IActionResult> Index()
        {
            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                return this.RedirectToAction("Index", "Dashboard");
            }

            var templates = await this.templateService.GetAllForCompany(companyId)
                .OrderByDescending(t => t.IsDefault)
                .ThenByDescending(t => t.CreatedOn)
                .ToListAsync();

            return this.View(templates);
        }

        public IActionResult Create()
        {
            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyId))
            {
                this.TempData["Error"] = "Company information not found. Please ensure you are properly authenticated.";
                return this.RedirectToAction("Index", "Home");
            }

            var model = new ApplicationTemplateViewModel
            {
                CompanyId = companyId,
                HtmlTemplate = this.GetDefaultTemplateHtml(),
                CssStyles = this.GetDefaultTemplateCss(),
            };

            this.ViewBag.Placeholders = this.placeholderService.GetAvailablePlaceholders();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationTemplateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.Placeholders = this.placeholderService.GetAvailablePlaceholders();
                return this.View(model);
            }

            var template = new ApplicationTemplate
            {
                CompanyId = model.CompanyId,
                TemplateName = model.TemplateName,
                HtmlTemplate = model.HtmlTemplate,
                CssStyles = model.CssStyles,
                JavaScript = model.JavaScript,
                Description = model.Description,
                IsDefault = model.IsDefault,
                Version = 1,
            };

            await this.templateService.CreateAsync(template);
            this.TempData["Success"] = "Template created successfully!";

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var template = await this.templateService.GetByIdAsync(id);
            if (template == null)
            {
                this.TempData["Error"] = "Template not found.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyId) || template.CompanyId != companyId)
            {
                this.TempData["Error"] = "You don't have permission to edit this template.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var model = new ApplicationTemplateViewModel
            {
                Id = template.Id,
                CompanyId = template.CompanyId,
                TemplateName = template.TemplateName,
                HtmlTemplate = template.HtmlTemplate,
                CssStyles = template.CssStyles,
                JavaScript = template.JavaScript,
                Description = template.Description,
                IsDefault = template.IsDefault,
                Version = template.Version,
            };

            this.ViewBag.Placeholders = this.placeholderService.GetAvailablePlaceholders();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationTemplateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewBag.Placeholders = this.placeholderService.GetAvailablePlaceholders();
                return this.View(model);
            }

            var template = await this.templateService.GetByIdAsync(model.Id);
            if (template == null)
            {
                return this.NotFound();
            }

            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (template.CompanyId != companyId)
            {
                return this.Unauthorized();
            }

            template.TemplateName = model.TemplateName;
            template.HtmlTemplate = model.HtmlTemplate;
            template.CssStyles = model.CssStyles;
            template.JavaScript = model.JavaScript;
            template.Description = model.Description;
            template.IsDefault = model.IsDefault;
            template.Version++;

            await this.templateService.UpdateAsync(template);
            this.TempData["Success"] = "Template updated successfully!";

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var template = await this.templateService.GetByIdAsync(id);
            if (template == null)
            {
                return this.NotFound();
            }

            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (template.CompanyId != companyId)
            {
                return this.Unauthorized();
            }

            await this.templateService.DeleteAsync(id);
            this.TempData["Success"] = "Template deleted successfully!";

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clone(string id, string newName)
        {
            var template = await this.templateService.GetByIdAsync(id);
            if (template == null)
            {
                return this.NotFound();
            }

            var companyId = this.User.FindFirst("CompanyId")?.Value;
            if (template.CompanyId != companyId)
            {
                return this.Unauthorized();
            }

            await this.templateService.CloneTemplateAsync(id, newName);
            this.TempData["Success"] = "Template cloned successfully!";

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult PlaceholderReference()
        {
            var placeholders = this.placeholderService.GetAvailablePlaceholders();
            return this.View(placeholders);
        }

        private string GetDefaultTemplateHtml()
        {
            // Return a starter template with placeholders
            return @"<div class=""application"">
                <div class=""d-flex justify-content-between"">
                    <div>
                        <h1>{{CompanyName}}</h1>
                    </div>
                    <div style=""text-align: right"">
                        <p><b>ВЪЗЛОЖИТЕЛ</b></p>
                        <p>{{CompanyName}}</p>
                        <p>{{CompanyAddress}}, {{CompanyCity}}, {{CompanyCountry}}</p>
                        <p>ИД. Код: {{CompanyTaxNumber}}</p>
                        <p>МОЛ: {{CompanyMOL}}</p>
                        <p>Тел: {{CompanyPhone}}</p>
                    </div>
                </div>

                <div style=""text-align: center"">
                    <h2>ЗАЯВКА</h2>
                    <p>за международен/вътреобщностен автомобилен транспорт</p>
                    <p>№ {{OrderNumber}} / {{OrderDate}}</p>
                </div>

                <p>Изпълнител:</p>
                <div style=""padding-left: 3rem;"">
                    <p>{{CarrierName}}</p>
                    <p>{{CarrierAddress}}</p>
                    <p>ИН по ЗДДС: {{CarrierTaxNumber}}</p>
                    <p>МОЛ: {{CarrierMOL}}</p>
                    <p>Тел: {{CarrierPhone}}</p>
                </div>

                {{#OrderTos}}
                <div class=""order-section"">
                    <p><b>· {{VehicleRegNumber}} {{VehicleTrailerRegNumber}}</b></p>
        
                    <table class=""table table-bordered"">
                        {{#Actions}}
                        <tr>
                            <td>{{ActionType}}</td>
                            <td>{{ActionAddress}}</td>
                            <td>{{ActionDate}}</td>
                            <td>{{ActionDetails}}</td>
                        </tr>
                        {{/Actions}}
                    </table>

                    <p>Данни за товара:</p>
                    <p>· Стока: <b>{{CargoName}} - {{CargoWeight}} т.</b></p>

                    <p>· Придружаващи документи:</p>
                    <div>
                        {{CMR}} CMR<br>
                        {{BillOfLading}} Bill of lading<br>
                        {{DeliveryNote}} Delivery note<br>
                        {{Invoice}} Invoice
                    </div>

                    <p>Цена и начин на плащане: <b>{{Price}}</b> {{Currency}} {{NoVAT}}</p>
                    <hr/>
                </div>
                {{/OrderTos}}

                <p>· Начин и срок на плащане: В {{DueDays}} дневен срок</p>
            </div>";
        }

        private string GetDefaultTemplateCss()
        {
            return @".application {
                font-size: 19px;
                font-family: Arial, sans-serif;
            }

            .head {
                margin-bottom: 0px;
                font-size: 14px;
            }

            body p {
                margin-bottom: 4px;
            }

            tr td {
                font-weight: bold;
            }

            .order-section {
                margin-bottom: 2rem;
                border: 1px solid #ddd;
                padding: 1rem;
            }";
        }
    }
}
