namespace BCKFreightTMS.Web.Controllers
{
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize(Roles = "SuperUser")]
    public class TransactionsController : Controller
    {
        private IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private IDeletableEntityRepository<InvoiceOut> invoiceOuts;
        private IOrdersService ordersService;
        private INotyfService notyfService;
        private IInvoicesService invoicesService;
        private IStringLocalizer<OrdersController> localizer;

        public TransactionsController(
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
    }
}
