﻿namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using BCKFreightTMS.Web.ViewModels.Shared;
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
            var invoices = this.invoicesService.LoadInvoiceInList(i => i.Status.Name != InvoiceStatusNames.AwaitingApproval.ToString());

            return this.View(invoices);
        }

        public IActionResult Incomplete()
        {
            var invoices = this.invoicesService.LoadInvoiceInList(i => i.Status.Name == InvoiceStatusNames.AwaitingApproval.ToString());

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

        public IActionResult Addition(string id)
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
            var viewModel = this.invoicesService.GetOrderTosInvoicing(id);

            return this.View(viewModel);
        }

        public IActionResult Unfinished()
        {
            var invoices = this.invoicesService.LoadInvoiceOutList(i => i.Status.Name == InvoiceStatusNames.AwaitingApproval.ToString());

            return this.View(invoices);
        }

        public IActionResult AwaitingPayment()
        {
            var invoices = this.invoicesService.LoadInvoiceOutList(i => i.Status.Name != InvoiceStatusNames.AwaitingApproval.ToString());

            return this.View(invoices);
        }

        public async Task<IActionResult> GenerateInvoice(string id)
        {
            var invHtml = await this.invoicesService.GenerateInvoiceHtml(id);
            var invModel = new InvoicePreview { Html = invHtml, InvoiceId = id };
            return this.View(invModel);
        }

        public async Task<IActionResult> DownloadInvoice(string id)
        {
            var pdfData = await this.invoicesService.GenerateInvoicePdf(id);
            return this.File(pdfData, GlobalConstants.PdfMimeType, $"Invoice.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> SaveIn(InvoiceInEditModel input)
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
                await this.invoicesService.UpdateInvoiceInStatusAsync(invoiceId, InvoiceStatusNames.AwaitingPayment.ToString());
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
            var invoiceId = await this.invoicesService.CreateInvoiceOut(input);
            await this.invoicesService.UpdateInvoiceOutStatusAsync(invoiceId, InvoiceStatusNames.AwaitingApproval.ToString());
            this.notyfService.Success(this.localizer["Invoice filed."]);

            return this.RedirectToAction("ForInvoicing");
        }

        [HttpPost]
        public async Task<IActionResult> FinishOut(InvoiceOutInputModel input)
        {
            var invoiceId = await this.invoicesService.CreateInvoiceOut(input);
            await this.invoicesService.UpdateInvoiceOutStatusAsync(invoiceId, InvoiceStatusNames.AwaitingPayment.ToString());
            this.notyfService.Success(this.localizer["Invoice filed."]);

            return this.Redirect(@$"/Invoices/GenerateInvoice/{invoiceId}");
        }

        [HttpPost]
        public async Task<IActionResult> FinishAddition(InvoiceOutEditModel input)
        {
            var invoiceId = await this.invoicesService.SaveInvoiceOut(input);
            await this.invoicesService.UpdateInvoiceOutStatusAsync(invoiceId, InvoiceStatusNames.AwaitingPayment.ToString());
            this.notyfService.Success(this.localizer["Invoice filed."]);

            return this.Redirect(@$"/Invoices/GenerateInvoice/{invoiceId}");
        }

        [HttpPost]
        public async Task<IActionResult> SaveOut(InvoiceOutEditModel input)
        {
            await this.invoicesService.SaveInvoiceOut(input);

            return this.RedirectToAction("Unfinished");
        }

        public IActionResult InvoiceNoteOut(string id)
        {
            var model = this.invoicesService.LoadInvoiceNoteOutModel(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InvoiceNoteOut(InvoiceNoteOutInputModel input)
        {
            try
            {
                var noteInvoiceId = await this.invoicesService.CreateInvoiceNoteOut(input);
                await this.invoicesService.UpdateInvoiceOutStatusAsync(noteInvoiceId, InvoiceStatusNames.AwaitingApproval.ToString());
            }
            catch (Exception ex)
            {
                this.notyfService.Error(ex.Message);
                return this.View(this.invoicesService.LoadInvoiceNoteOutModel(input.InvoiceOut.Id));
            }

            return this.RedirectToAction("AwaitingPayment");
        }
    }
}
