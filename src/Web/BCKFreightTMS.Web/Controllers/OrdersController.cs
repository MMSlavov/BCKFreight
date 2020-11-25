namespace BCKFreightTMS.Web.Controllers
{
    using System.Collections.Generic;

    using BCKFreightTMS.Data.Models;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        public IActionResult Index()
        {
            var orders = new List<Order> {};
            return this.View(orders);
        }
    }
}
