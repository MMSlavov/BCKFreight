namespace BCKFreightTMS.Web.Controllers
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class OrdersController : BaseController
    {
        public IActionResult List()
        {
            var orders = new List<Order> {};
            return this.View(orders);
        }
    }
}
