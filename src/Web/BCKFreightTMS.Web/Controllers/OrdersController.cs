namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

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
        private readonly IEmailSender emailSender;

        public OrdersController(
            IDeletableEntityRepository<Order> orders,
            IDeletableEntityRepository<Currency> currencies,
            IDeletableEntityRepository<OrderStatus> orderStatuses,
            IOrdersService ordersService,
            IFinanceService financeService,
            IPdfService pdfService,
            IViewRenderService viewRenderService,
            IEmailSender emailSender)
        {
            this.orders = orders;
            this.currencies = currencies;
            this.orderStatuses = orderStatuses;
            this.ordersService = ordersService;
            this.financeService = financeService;
            this.pdfService = pdfService;
            this.viewRenderService = viewRenderService;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>(o => o.Status.Name != OrderStatusNames.Accepted.ToString() &&
                                                                            o.Status.Name != OrderStatusNames.Ready.ToString() &&
                                                                            o.Status.Name != OrderStatusNames.Finished.ToString())
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

        public IActionResult Finished()
        {
            var orders = this.ordersService.GetAll<ListFinishedOrderViewModel>(o =>
                                            o.Status.Name == OrderStatusNames.Finished.ToString())
                                           .ToList();
            return this.View(orders);
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

        public async Task<IActionResult> CorrectApplication(string id)
        {
            var appModel = await this.LoadApplicationModel(id);
            return this.View(appModel);
        }

        public async Task<IActionResult> DownloadApplication(string id)
        {
            var model = this.ordersService.GenerateApplicationModel(id);
            var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
            var pdfData = this.pdfService.SelectPdfConvert(html);
            return this.File(pdfData, GlobalConstants.PdfMimeType, $"OrderContract{model.OrderToReferenceNum}.pdf");
        }

        public async Task<IActionResult> BeginOrder(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name != OrderStatusNames.Ready.ToString())
            {
                return this.NotFound();
            }

            await this.ordersService.BeginAsync(id);

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
                input = this.ordersService.LoadOrderAcceptInputModel(input);
                return this.View(input);
            }

            await this.ordersService.AcceptAsync(input, this.User);
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

            var priceIn = this.financeService.GetAmount(input.OrderFromCurrencyId, input.OrderFromPriceNetIn);
            var priceOut = this.financeService.GetAmount(input.CurrencyOutId, input.PriceNetOut);
            var margin = priceIn - priceOut;
            var fromCurrencyId = this.currencies.AllAsNoTracking().FirstOrDefault(c => c.Name == CurrencyCodes.EUR.ToString()).Id;
            if (priceIn < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMaxAmount) &&
                margin < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMinMargin))
            {
                this.ModelState.AddModelError(string.Empty, "The order margin for order under 500€ cannot be less than 25€.");
                input = this.ordersService.LoadOrderCreateInputModel(input.Id);
                return this.View(input);
            }

            var minMarginPer = priceIn * GlobalConstants.MinOrderMargin;

            if (margin < minMarginPer)
            {
                this.ModelState.AddModelError(string.Empty, "The order margin cannot be less than 5%.");
                input = this.ordersService.LoadOrderCreateInputModel(input.Id);
                return this.View(input);
            }

            await this.ordersService.CreateAsync(input);
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

            var priceIn = this.financeService.GetAmount(input.OrderFromCurrencyId, input.OrderFromPriceNetIn);
            var priceOut = this.financeService.GetAmount(input.OrderToCurrencyId, input.OrderToPriceNetOut);
            var margin = priceIn - priceOut;
            var fromCurrencyId = this.currencies.AllAsNoTracking().FirstOrDefault(c => c.Name == CurrencyCodes.EUR.ToString()).Id;
            if (priceIn < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMaxAmount) &&
                margin < this.financeService.GetAmount(fromCurrencyId, GlobalConstants.SmallOrderMinMargin))
            {
                this.ModelState.AddModelError(string.Empty, "The order margin for order under 500€ cannot be less than 25€.");
                input = this.ordersService.LoadOrderEditInputModel(input.Id);
                return this.View(input);
            }

            var minMarginPer = priceIn * GlobalConstants.MinOrderMargin;

            if (margin < minMarginPer)
            {
                this.ModelState.AddModelError(string.Empty, "The order margin cannot be less than 5%.");
                input = this.ordersService.LoadOrderEditInputModel(input.Id);
                return this.View(input);
            }

            await this.ordersService.EditAsync(input);

            // await this.SendContractToCompany(input.Id);
            // send or download application
            return this.Redirect(@$"/Orders/CorrectApplication/{input.Id}");
        }

        private async Task SendContractToCompany(string companyId)
        {
            var model = this.ordersService.GenerateApplicationModel(companyId);
            var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
            var pdfData = this.pdfService.PdfSharpConvert(html);
            var pdf = new EmailAttachment
            {
                Content = pdfData,
                FileName = $"OrderApplication{model.OrderToReferenceNum}.pdf",
                MimeType = GlobalConstants.PdfMimeType,
            };
            var email = System.IO.File.ReadAllText(@"wwwroot\data\Email.html");
            await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    "mariobanya66@gmail.com",
                    "Order Contract",
                    email,
                    new[] { pdf });
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.ordersService.DeleteAsync(id))
            {
                this.ModelState.AddModelError(string.Empty, "Order delete fail.");
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Edit(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name == OrderStatusNames.Finished.ToString())
            {
                return this.RedirectToAction(GlobalConstants.Index);
            }

            var model = this.ordersService.LoadOrderEditInputModel(id);

            return this.View(model);
        }

        public IActionResult Status(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name != OrderStatusNames.InProgress.ToString())
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
            return this.RedirectToAction(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(OrderStatusViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Status", this.ordersService.LoadOrderStatusModel(input.Id));
            }

            if (input.Actions.Any(a => !a.IsFinnished))
            {
                this.ModelState.AddModelError(string.Empty, "All actions must be completed to finish order.");
                return this.RedirectToAction("Status", this.ordersService.LoadOrderStatusModel(input.Id));
            }

            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            order.StatusId = this.orderStatuses.AllAsNoTracking()
                                   .FirstOrDefault(s => s.Name == OrderStatusNames.DocumentationCheck.ToString())
                                   .Id;
            await this.ordersService.UpdateOrderStatusAsync(input);
            var model = this.ordersService.LoadOrderFinishModel(input.Id);

            return this.View(model);
        }

        public IActionResult Finish(string id)
        {
            var model = this.ordersService.LoadOrderFinishModel(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinishOrder(OrderFinishViewModel input)
        {
            if (!this.ordersService.ValidateFinishModel(input) &&
                input.OrderStatus.StatusName != OrderStatusNames.Approved.ToString())
            {
                await this.ordersService.MarkOrderForApproval(input);
            }
            else
            {
                await this.ordersService.FinishOrderAsync(input);
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public async Task<IActionResult> ApproveDocumentation(OrderFinishViewModel input)
        {
            await this.ordersService.ApproveOrder(input);

            return this.RedirectToAction("Finish", "Orders", new { id = input.OrderStatus.Id });
        }

        private async Task<ApplicationModel> LoadApplicationModel(string orderId)
        {
            var model = this.ordersService.GenerateApplicationModel(orderId);
            var html = await this.viewRenderService.RenderToStringAsync("Orders/Application", model);
            var appModel = new ApplicationModel { ApplicationHtml = html, OrderId = orderId };
            return appModel;
        }
    }
}
