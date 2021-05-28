namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using AutoMapper;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Transactions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Localization;

    [Authorize(Roles = "SuperUser")]
    public class TransactionsController : Controller
    {
        private readonly IDeletableEntityRepository<AccountingType> accountingTypes;
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly IDeletableEntityRepository<BankMovement> bankMovements;
        private readonly IMapper mapper;
        private IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private IDeletableEntityRepository<InvoiceOut> invoiceOuts;
        private IOrdersService ordersService;
        private INotyfService notyfService;
        private IInvoicesService invoicesService;
        private IStringLocalizer<OrdersController> localizer;

        public TransactionsController(
                    IDeletableEntityRepository<InvoiceIn> invoiceIns,
                    IDeletableEntityRepository<InvoiceOut> invoiceOuts,
                    IDeletableEntityRepository<AccountingType> accountingTypes,
                    IDeletableEntityRepository<Company> companies,
                    IDeletableEntityRepository<BankMovement> bankMovements,
                    IMapper mapper,
                    IOrdersService ordersService,
                    INotyfService notyfService,
                    IInvoicesService invoicesService,
                    IStringLocalizer<OrdersController> localizer)
        {
            this.invoiceIns = invoiceIns;
            this.invoiceOuts = invoiceOuts;
            this.accountingTypes = accountingTypes;
            this.companies = companies;
            this.bankMovements = bankMovements;
            this.mapper = mapper;
            this.ordersService = ordersService;
            this.notyfService = notyfService;
            this.invoicesService = invoicesService;
            this.localizer = localizer;
        }

        public IActionResult PendingIn()
        {
            var invoices = this.invoicesService.LoadInvoiceInList(i => i.Status.Name == InvoiceStatusNames.AwaitingPayment.ToString() ||
                                                                     i.Status.Name == InvoiceStatusNames.PayAttempt.ToString());

            return this.View(invoices);
        }

        public IActionResult PaidIn()
        {
            var invoices = this.invoicesService.LoadInvoiceInList(i => i.Status.Name == InvoiceStatusNames.Paid.ToString());

            return this.View(invoices);
        }

        public IActionResult PendingOut()
        {
            var invoices = this.invoicesService.LoadInvoiceOutList(i => i.Status.Name == InvoiceStatusNames.AwaitingPayment.ToString());

            return this.View(invoices);
        }

        public async Task<IActionResult> PayInAsync(string id)
        {
            await this.invoicesService.PayInvoiceIn(id);

            return this.RedirectToAction(nameof(this.PendingIn));
        }

        public async Task<IActionResult> PayAsync(string id)
        {
            await this.invoicesService.UpdateInvoiceInStatusAsync(id, InvoiceStatusNames.PayAttempt.ToString());

            return this.RedirectToAction(nameof(this.PendingIn));
        }

        public IActionResult PaidOut()
        {
            var invoices = this.invoicesService.LoadInvoiceOutList(i => i.Status.Name == InvoiceStatusNames.Paid.ToString());

            return this.View(invoices);
        }

        public async Task<IActionResult> PayOut(string id)
        {
            await this.invoicesService.PayInvoiceOut(id);

            return this.RedirectToAction(nameof(this.PendingOut));
        }

        public IActionResult Accounting()
        {
            var model = new AccountingModel();
            model.CreditTypeItems = this.accountingTypes.All().Where(t => t.MovementType == "Credit").To<AccountingTypeModel>().ToList();
            model.DebitTypeItems = this.accountingTypes.All().Where(t => t.MovementType == "Debit").To<AccountingTypeModel>().ToList();
            model.CompanyItems = this.companies.All()
                                     .Select(c => new SelectListItem { Text = c.Name, Value = c.Id })
                                     .ToList();
            model.BankItems = Enum.GetValues(typeof(BankCodes)).Cast<BankCodes>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();

            return this.View(model);
        }

        [HttpPost]
        public IActionResult ProcessBS(IFormFile file)
        {
            var serializer = new XmlSerializer(typeof(DSKStatementModel));

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var xmlText = reader.ReadToEnd();
            xmlText = xmlText.Replace("</br>", " ");
            xmlText = Regex.Replace(xmlText, @"(?<=\d)\,(?=\d)", ".");

            using var strReader = new StringReader(xmlText);
            var model = (DSKStatementModel)serializer.Deserialize(strReader);

            return this.View(model);
        }

        [HttpPost]
        public IActionResult ProcessBSJson(IFormFile file, string bankCode)
        {
            var asmbl = typeof(BCKFreightTMS.Web.ViewModels.Transactions.BankStatementModel).Assembly;
            var type = asmbl.GetTypes().FirstOrDefault(t => t.Name.Contains(bankCode));
            var serializer = new XmlSerializer(type);

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            var xmlText = reader.ReadToEnd();
            xmlText = xmlText.Replace("</br>", "\n");
            xmlText = Regex.Replace(xmlText, @"(?<=\d)\,(?=\d)", ".");

            using var strReader = new StringReader(xmlText);
            var model = serializer.Deserialize(strReader);

            var modelMap = this.mapper.Map<BankStatementModel>(model);

            return this.Json(modelMap.Movements);
        }

        public JsonResult SearchCompany(string companyName, string companyIban)
        {
            if (companyName == null && companyIban == null)
            {
                return this.Json(new { Id = string.Empty });
            }

            var company = this.companies.All().ToList().FirstOrDefault(c => c.BankDetails.Any(bd => bd.BankIban.Contains(companyIban ?? string.Empty, StringComparison.InvariantCulture)) ||
                                                                            c.Name.Contains(companyName ?? string.Empty, StringComparison.InvariantCultureIgnoreCase));
            if (company != null)
            {
                return this.Json(new { Id = company.Id });
            }

            return this.Json(new { Id = string.Empty });
        }

        public JsonResult GetCompanyInvoices(string companyId, string mvType)
        {
            if (mvType == "Credit")
            {
                var invoicesOut = this.invoicesService.LoadInvoiceOutList(i => i.OrderTos.First().Order.OrderFrom.Company.Id == (companyId ?? string.Empty) && i.Status.Name == InvoiceStatusNames.AwaitingPayment.ToString());
                foreach (var invoice in invoicesOut)
                {
                    invoice.CreateDate = invoice.CreateDate.ToLocalTime();
                }

                return this.Json(invoicesOut);
            }

            var invoicesIn = this.invoicesService.LoadInvoiceInList(i => i.BankDetails.Company.Id == (companyId ?? string.Empty));
            foreach (var invoice in invoicesIn)
            {
                invoice.CreateDate = invoice.CreateDate.ToLocalTime();
            }

            return this.Json(invoicesIn);
        }

        [HttpPost]
        public async Task SafeBankMovement(BankMovementInputModel input)
        {
            var movement = new BankMovement
            {
                Date = input.DateIn,
                Reason = input.ReasonIn,
                OppositeSideName = input.OSNameIn,
                OppositeSideAccount = input.OSAccIn,
                Amount = input.AmountIn,
                AccountingTypeId = input.AccTypeId,
            };

            await this.bankMovements.AddAsync(movement);
            await this.bankMovements.SaveChangesAsync();
        }
    }
}
