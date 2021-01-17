namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Orders;
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

        public OrdersController(
            IDeletableEntityRepository<Order> orders,
            IDeletableEntityRepository<Currency> currencies,
            IDeletableEntityRepository<OrderStatus> orderStatuses,
            IOrdersService ordersService,
            IFinanceService financeService)
        {
            this.orders = orders;
            this.currencies = currencies;
            this.orderStatuses = orderStatuses;
            this.ordersService = ordersService;
            this.financeService = financeService;
        }

        public IActionResult Index()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>(o => o.Status.Name != OrderStatusNames.Accepted.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public new IActionResult Accepted()
        {
            var orders = this.ordersService.GetAll<ListAcceptedOrderViewModel>(o => o.Status.Name == OrderStatusNames.Accepted.ToString())
                                           .ToList();
            return this.View(orders);
        }

        public IActionResult Accept()
        {
            var model = this.ordersService.LoadOrderAcceptInputModel();
            return this.View(model);
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
            return this.RedirectToAction(GlobalConstants.Index);
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
            return this.RedirectToAction(GlobalConstants.Index);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.ordersService.DeleteAsync(id))
            {
                this.ModelState.AddModelError(string.Empty, "Order delete fail.");
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Status(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name == OrderStatusNames.Finished.ToString())
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
    }
}
