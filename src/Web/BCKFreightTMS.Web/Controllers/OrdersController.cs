namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class OrdersController : BaseController
    {
        private readonly IDeletableEntityRepository<Order> orders;
        private readonly IOrdersService ordersService;

        public OrdersController(
            IDeletableEntityRepository<Order> orders,
            IOrdersService ordersService)
        {
            this.orders = orders;
            this.ordersService = ordersService;
        }

        public IActionResult Index()
        {
            var orders = this.ordersService.GetAll<ListOrderViewModel>();
            return this.View(orders);
        }

        public IActionResult Create()
        {
            var model = this.ordersService.LoadOrderInputModel();
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

        [HttpPost]
        public async Task<IActionResult> Create(OrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.ordersService.LoadOrderInputModel(input);
                return this.View(input);
            }

            await this.ordersService.CreateAsync(input, this.User);
            return this.RedirectToAction(GlobalConstants.Index);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.ordersService.DeleteAsync(id))
            {
                this.ModelState.AddModelError(string.Empty, "Order do not exist.");
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

            await this.ordersService.FinishOrderAsync(input);

            return this.RedirectToAction(GlobalConstants.Index);
        }
    }
}
