namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetCoreHero.ToastNotification.Abstractions;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Messaging;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using BCKFreightTMS.Web.ViewModels.Shared;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize(Roles = "User")]
    public class OrdersController : BaseController
    {
        private readonly IDeletableEntityRepository<Order> orders;
        private readonly IDeletableEntityRepository<Currency> currencies;
        private readonly IDeletableEntityRepository<OrderStatus> orderStatuses;
        private readonly IOrdersService ordersService;
        private readonly IFinanceService financeService;
        private readonly IPdfService pdfService;
        private readonly IViewRenderService viewRenderService;
        private readonly IContactsService contactsService;
        private readonly INotyfService notyfService;
        private readonly IStringLocalizer<OrdersController> localizer;

        public OrdersController(
            IDeletableEntityRepository<Order> orders,
            IDeletableEntityRepository<Currency> currencies,
            IDeletableEntityRepository<OrderStatus> orderStatuses,
            IOrdersService ordersService,
            IFinanceService financeService,
            IPdfService pdfService,
            IViewRenderService viewRenderService,
            IContactsService contactsService,
            INotyfService notyfService,
            IStringLocalizer<OrdersController> localizer)
        {
            this.orders = orders;
            this.currencies = currencies;
            this.orderStatuses = orderStatuses;
            this.ordersService = ordersService;
            this.financeService = financeService;
            this.pdfService = pdfService;
            this.viewRenderService = viewRenderService;
            this.contactsService = contactsService;
            this.notyfService = notyfService;
            this.localizer = localizer;
        }

        public IActionResult Index()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>(o =>
                                            o.Status.Name != OrderStatusNames.Accepted.ToString() &&
                                            o.Status.Name != OrderStatusNames.Ready.ToString() &&
                                            o.Status.Name != OrderStatusNames.Finished.ToString() &&
                                            o.Status.Name != OrderStatusNames.DocumentationCheck.ToString() &&
                                            o.Status.Name != OrderStatusNames.AwaitingApproval.ToString() &&
                                            o.Status.Name != OrderStatusNames.Approved.ToString() &&
                                            o.Status.Name != OrderStatusNames.AwaitingApplication.ToString() &&
                                            o.Status.Name != OrderStatusNames.Fail.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult WaitingConfirm()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.AwaitingApplication.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public new IActionResult Accepted()
        {
            var orders = this.ordersService.GetAll<ListAcceptedOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.Accepted.ToString() ||
                                            o.Status.Name == OrderStatusNames.Ready.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Check()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.DocumentationCheck.ToString() ||
                                            o.Status.Name == OrderStatusNames.AwaitingApproval.ToString() ||
                                            o.Status.Name == OrderStatusNames.Approved.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Finished()
        {
            var orders = this.ordersService.GetAll<ListFinishedOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.Finished.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Failed()
        {
            var orders = this.ordersService.GetAll<ListFailedOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.Fail.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Details(string id)
        {
            var data = new Dictionary<string, string>();

            var order = this.orders.All().FirstOrDefault(c => c.Id == id);
            var orderTos = order.OrderTos.ToList();
            data.Add("Id", order.Id);
            data.Add("Reference number to", order.ReferenceNum);
            data.Add("Reference number from", order.OrderFrom?.ReferenceNum);
            data.Add("Client company", order.OrderFrom?.Company.Name);
            data.Add("Client price", order.OrderFrom?.PriceNetIn.ToString());
            data.Add("Client currency", order.OrderFrom?.Currency.Name);
            data.Add("Client contact", order.OrderFrom?.Contact?.FirstName);

            for (int i = 0; i < orderTos.Count; i++)
            {
                data.Add($"C{i + 1} Carrier vehicle", orderTos[i].Vehicle.RegNumber);
                data.Add($"C{i + 1} Carrier company", orderTos[i].Company.Name);
                data.Add($"C{i + 1} Carrier price", orderTos[i].PriceNetOut.ToString());
                data.Add($"C{i + 1} Carrier currency", orderTos[i].Currency.Name);
                data.Add($"C{i + 1} Carrier contact", orderTos[i].Contact?.FirstName);
                data.Add($"C{i + 1} Carrier driver", orderTos[i].Drivers.FirstOrDefault()?.Driver.FirstName);
                data.Add($"C{i + 1} Cargo name", orderTos[i].Cargo.Name);
                data.Add($"C{i + 1} Cargo weight", orderTos[i].Cargo.WeightGross.ToString());
                data.Add($"C{i + 1} Cargo quantity", orderTos[i].Cargo.Quantity.ToString());
                data.Add($"C{i + 1} Cargo requaired loading body", orderTos[i].Cargo.LoadingBody?.Name);
            }

            data = data.Where(kv => kv.Value != null).ToDictionary(x => x.Key, y => y.Value);

            return this.View(data);
        }

        public IActionResult Accept()
        {
            var model = this.ordersService.LoadOrderAcceptInputModel();
            return this.View(model);
        }

        public async Task<IActionResult> GenerateApplication(string id)
        {
            var appModel = await this.LoadApplicationModel(id);
            return this.View(appModel);
        }

        public async Task<IActionResult> CorrectApplication(string id, string returnUrl = null)
        {
            var appModel = await this.LoadApplicationModel(id);
            if (returnUrl is not null)
            {
                appModel.ReturnUrl = returnUrl;
            }

            return this.View(appModel);
        }

        public async Task<IActionResult> DownloadApplication(string id)
        {
            var model = this.ordersService.GenerateApplicationModel(id);
            var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
            var pdfData = this.pdfService.SelectPdfConvert(html);
            return this.File(pdfData, GlobalConstants.PdfMimeType, $"OrderContract{model.ReferenceNum}.pdf");
        }

        public async Task<IActionResult> BeginOrder(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name != OrderStatusNames.Ready.ToString())
            {
                this.notyfService.Error(this.localizer["Invalid order!"]);
                return this.Redirect("/Orders/Accepted");
            }

            await this.ordersService.BeginAsync(id);

            // send
            this.notyfService.Success(this.localizer["Order contract sent."]);
            return this.RedirectToAction("WaitingConfirm");
        }

        public async Task<IActionResult> ConfirmOrderApplication(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name != OrderStatusNames.AwaitingApplication.ToString())
            {
                this.notyfService.Error(this.localizer["Invalid order!"]);
                return this.RedirectToAction("/Orders/WaitingConfirm");
            }

            await this.ordersService.ConfirmApplicationAsync(id);

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Create(string id)
        {
            var model = this.ordersService.LoadOrderCreateInputModel(id);
            return this.View(model);
        }

        public JsonResult GetContacts(string companyId)
        {
            var contacts = this.ordersService.GetContacts(companyId);
            return this.Json(contacts);
        }

        public JsonResult GetDrivers(string companyId)
        {
            var drivers = this.ordersService.GetDrivers(companyId);
            return this.Json(drivers);
        }

        public JsonResult GetVehicles(string companyId)
        {
            var vehicles = this.ordersService.GetVehicles(companyId);
            return this.Json(vehicles);
        }

        public JsonResult GetTrailers(string companyId)
        {
            var vehicles = this.ordersService.GetTrailers(companyId);
            return this.Json(vehicles);
        }

        public JsonResult GetCarriersByArea(string area)
        {
            var carriers = this.ordersService.GetCarriersByArea(area);
            return this.Json(carriers);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(OrderAcceptInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        this.notyfService.Error(this.localizer[error.ErrorMessage]);
                    }
                }

                input = this.ordersService.LoadOrderAcceptInputModel(input);
                return this.View(input);
            }

            await this.ordersService.AcceptAsync(input, this.User);
            this.notyfService.Success(this.localizer["Order accepted."]);
            return this.RedirectToAction("Accepted");
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.ordersService.LoadOrderCreateInputModel(input.Id);
                return this.View(input);
            }

            // var priceIn = this.financeService.GetAmount(input.OrderFromCurrencyId, input.OrderFromPriceNetIn);
            // var priceOut = this.financeService.GetAmount(input.CurrencyOutId, input.PriceNetOut);
            // var margin = priceIn - priceOut;
            // var fromCurrencyId = this.currencies.AllAsNoTracking().FirstOrDefault(c => c.Name == CurrencyCodes.EUR.ToString()).Id;

            //// if (priceIn < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMaxAmount) &&
            ////    margin < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMinMargin))
            //// {
            ////    this.ModelState.AddModelError(string.Empty, "The order margin for order under 500€ cannot be less than 25€.");
            ////    input = this.ordersService.LoadOrderCreateInputModel(input.Id);
            ////    return this.View(input);
            //// }
            // var minMarginPer = priceIn * GlobalConstants.MinOrderMargin;

            // if (margin < minMarginPer)
            // {
            //    this.ModelState.AddModelError(string.Empty, "The order margin cannot be less than 5%.");
            //    input = this.ordersService.LoadOrderCreateInputModel(input.Id);
            //    return this.View(input);
            // }
            await this.ordersService.CreateAsync(input);
            this.notyfService.Success(this.localizer["Order carrier found."]);
            return this.Redirect(@$"/Orders/GenerateApplication/{input.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.ordersService.LoadOrderEditInputModel(input.Id);
                return this.View(input);
            }

            // var priceIn = this.financeService.GetAmount(input.OrderFromCurrencyId, input.OrderFromPriceNetIn);
            // var priceOut = this.financeService.GetAmount(input.OrderToCurrencyId, input.OrderToPriceNetOut);
            // var margin = priceIn - priceOut;
            // var fromCurrencyId = this.currencies.AllAsNoTracking().FirstOrDefault(c => c.Name == CurrencyCodes.EUR.ToString()).Id;
            //// if (priceIn < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMaxAmount) &&
            ////    margin < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMinMargin))
            //// {
            ////    this.ModelState.AddModelError(string.Empty, "The order margin for order under 500€ cannot be less than 25€.");
            ////    input = this.ordersService.LoadOrderEditInputModel(input.Id);
            ////    return this.View(input);
            //// }

            // var minMarginPer = priceIn * GlobalConstants.MinOrderMargin;

            // if (margin < minMarginPer)
            // {
            //    this.ModelState.AddModelError(string.Empty, "The order margin cannot be less than 5%.");
            //    input = this.ordersService.LoadOrderEditInputModel(input.Id);
            //    return this.View(input);
            // }
            await this.ordersService.EditAsync(input);

            // await this.SendContractToCompanyAsync(input.Id);
            this.notyfService.Success(this.localizer["Order contract sent."]);
            return this.Redirect(@$"/Orders/CorrectApplication/{input.Id}{(input.ReturnUrl is not null ? $"?returnUrl={input.ReturnUrl}" : string.Empty)}");
        }

        public async Task<IActionResult> Delete(string id, string returnUrl = null)
        {
            if (!await this.ordersService.DeleteAsync(id))
            {
                this.ModelState.AddModelError(string.Empty, "Order delete fail.");
            }

            if (returnUrl is not null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Fail(string id, string returnUrl = null)
        {
            var model = this.ordersService.LoadOrderFailModel(id);
            model.ReturnUrl = returnUrl;
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Fail(OrderFailModel input)
        {
            await this.ordersService.SetOrderFailAsync(input);

            this.notyfService.Error(this.localizer["Order failed!"]);
            if (input.ReturnUrl is not null)
            {
                return this.Redirect(input.ReturnUrl);
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Edit(string id, string returnUrl = null)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == id);
            if (order is null || order.Status.Name == OrderStatusNames.Finished.ToString())
            {
                if (returnUrl is not null)
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction(GlobalConstants.Index);
            }

            var model = this.ordersService.LoadOrderEditInputModel(id);
            if (returnUrl is not null)
            {
                model.ReturnUrl = returnUrl;
            }

            return this.View(model);
        }

        public IActionResult Status(string id)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == id);
            if (order is null || order.Status.Name == OrderStatusNames.Finished.ToString())
            {
                return this.RedirectToAction(GlobalConstants.Index);
            }

            var model = this.ordersService.LoadOrderStatusModel(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Status(OrderStatusViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(this.ordersService.LoadOrderStatusModel(input.Id));
            }

            await this.ordersService.UpdateOrderStatusAsync(input);
            this.notyfService.Success(this.localizer["Order updated."]);
            return this.RedirectToAction(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(OrderStatusViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Status", this.ordersService.LoadOrderStatusModel(input.Id));
            }

            await this.ordersService.UpdateOrderStatusAsync(input);

            if (this.orders.All().FirstOrDefault(o => o.Id == input.Id).OrderTos
                                 .SelectMany(o => o.OrderActions)
                                 .Any(a => !a.IsFinished))
            {
                this.ModelState.AddModelError(string.Empty, "All actions must be completed to finish order.");
                return this.RedirectToAction("Status", this.ordersService.LoadOrderStatusModel(input.Id));
            }

            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            order.StatusId = this.orderStatuses.AllAsNoTracking()
                                   .FirstOrDefault(s => s.Name == OrderStatusNames.DocumentationCheck.ToString())
                                   .Id;
            await this.orders.SaveChangesAsync();
            var model = this.ordersService.LoadOrderFinishModel(input.Id);

            return this.View(model);
        }

        public IActionResult Finish(string id)
        {
            var model = this.ordersService.LoadOrderFinishModel(id);

            return this.View(model);
        }

        public IActionResult ConfirmApplication(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name != OrderStatusNames.AwaitingApplication.ToString())
            {
                return this.NotFound();
            }

            return this.View(new ConfirmApplicationModel { OrderId = id });
        }

        [HttpPost]
        public async Task<IActionResult> FinishOrder(OrderFinishViewModel input)
        {
            if (!this.ordersService.ValidateFinishModel(input) &&
                input.OrderStatus.StatusName != OrderStatusNames.Approved.ToString())
            {
                await this.ordersService.MarkOrderForApproval(input);
                this.notyfService.Warning(this.localizer["Order marked for approval."]);
            }
            else
            {
                await this.ordersService.FinishOrderAsync(input);
                this.notyfService.Success(this.localizer["Order finished."]);
            }

            return this.RedirectToAction("Check");
        }

        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public async Task<IActionResult> ApproveDocumentation(OrderFinishViewModel input)
        {
            await this.ordersService.ApproveOrder(input);
            this.notyfService.Success(this.localizer["Order approved."]);

            return this.RedirectToAction("Finish", "Orders", new { id = input.OrderStatus.Id });
        }

        private async Task<ApplicationModel> LoadApplicationModel(string orderId)
        {
            var model = this.ordersService.GenerateApplicationModel(orderId);
            var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
            var appModel = new ApplicationModel { ApplicationHtml = html, OrderId = orderId };
            return appModel;
        }

        //private async Task SendContractToCompanyAsync(string orderId)
        //{
        //    var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
        //    if (order is null || order.Status.Name == OrderStatusNames.Finished.ToString())
        //    {
        //        throw new ArgumentException("Order do not exist");
        //    }

        //    var model = this.ordersService.GenerateApplicationModel(order.Id);
        //    var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
        //    var pdfData = this.pdfService.SelectPdfConvert(html);
        //    var pdf = new EmailAttachment
        //    {
        //        Content = pdfData,
        //        FileName = $"OrderApplication{model.OrderToReferenceNum}.pdf",
        //        MimeType = GlobalConstants.PdfMimeType,
        //    };
        //    var emailHtml = System.IO.File.ReadAllText(@"wwwroot\data\Email.html");
        //    await this.contactsService.SendEmailToCompanyAsync(
        //            order.OrderTo.Company.Id,
        //            "Order contract",
        //            emailHtml,
        //            new[] { pdf });
        //}
    }
}
