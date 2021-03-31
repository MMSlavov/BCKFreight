namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize(Roles = "User")]
    public class InvoicesController : Controller
    {
        private readonly IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private readonly IDeletableEntityRepository<InvoiceOut> invoiceOuts;
        private readonly IOrdersService ordersService;
        private readonly INotyfService notyfService;
        private readonly IInvoicesService invoicesService;
        private readonly IStringLocalizer<OrdersController> localizer;

        public InvoicesController(
            IDeletableEntityRepository<InvoiceIn> invoiceIns,
            IDeletableEntityRepository<InvoiceOut> invoiceOuts,
            IOrdersService ordersService,
            INotyfService notyfService,
            IInvoicesService invoicesService,
            IStringLocalizer<OrdersController> localizer)
        {
            this.invoiceIns = invoiceIns;
            this.invoiceOuts = invoiceOuts;
            this.ordersService = ordersService;
            this.notyfService = notyfService;
            this.invoicesService = invoicesService;
            this.localizer = localizer;
        }

        public IActionResult Filed()
        {
            var invoices = this.invoiceIns.All().Where(i => i.Status.Name != InvoiceStatusNames.AwaitingApproval.ToString())
                                              .To<ListInvoiceInModel>()
                                              .ToList();
            return this.View(invoices);
        }

        public IActionResult Incomplete()
        {
            var invoices = this.invoiceIns.All().Where(i => i.Status.Name == InvoiceStatusNames.AwaitingApproval.ToString())
                                              .To<ListInvoiceInModel>()
                                              .ToList();
            return this.View(invoices);
        }

        public IActionResult ForInvoicing()
        {
            var orders = this.ordersService.GetAllOrderTos<ListOrderToViewModel>(o => o.IsFinished &&
                                                                                      o.InvoiceInId != null &&
                                                                                      o.InvoiceOutId == null)
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Correct(string id)
        {
            var model = this.invoicesService.LoadInvoiceEditModel(id);

            return this.View(model);
        }

        public IActionResult EditIn(string id)
        {
            var model = this.invoicesService.LoadInvoiceEditModel(id);

            return this.View(model);
        }

        public IActionResult EditOut(string id)
        {
            var model = this.invoicesService.LoadInvoiceOutEditModel(id);

            return this.View(model);
        }

        public IActionResult Invoicing(string id)
        {
            var model = this.invoicesService.LoadInvoiceOutModel(id);

            return this.View(model);
        }

        public IActionResult GetOrderTo(string id)
        {
            var viewModel = this.ordersService.GetAllOrderTos<OrderToInvoiceModel>(o => o.Order.OrderFrom.CompanyId == id &&
                                                                                        o.IsFinished &&
                                                                                        o.InvoiceInId != null &&
                                                                                        o.InvoiceOutId == null);
            return this.View(viewModel);
        }

        public IActionResult Unfinished()
        {
            var invoices = this.invoiceOuts.All().Where(i => i.Status.Name == InvoiceStatusNames.AwaitingApproval.ToString())
                                                  .To<ListInvoiceOutModel>()
                                                  .ToList();
            return this.View(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> Save(InvoiceInEditModel input)
        {
            await this.invoicesService.SaveInvoiceIn(input);

            return this.RedirectToAction("Incomplete");
        }

        [HttpPost]
        public async Task<IActionResult> Finish(InvoiceInEditModel input)
        {
            var invoiceId = await this.invoicesService.SaveInvoiceIn(input);
            if (this.ordersService.ValidateFinishModel(input.OrderTos))
            {
                await this.invoicesService.UpdateInvoiceInStatus(invoiceId, InvoiceStatusNames.AwaitingPayment.ToString());
                this.notyfService.Success(this.localizer["Invoice filed."]);
            }
            else
            {
                this.notyfService.Error(this.localizer["Error"]);
                return this.RedirectToAction("Incomplete");
            }

            return this.RedirectToAction("Filed");
        }

        [HttpPost]
        public async Task<IActionResult> SaveForAddition(InvoiceOutInputModel input)
        {
            var invoiceId = await this.invoicesService.SaveInvoiceOut(input);
            await this.invoicesService.UpdateInvoiceOutStatus(invoiceId, InvoiceStatusNames.AwaitingApproval.ToString());
            this.notyfService.Success(this.localizer["Invoice filed."]);

            return this.RedirectToAction("ForInvoicing");
        }
    }
}
