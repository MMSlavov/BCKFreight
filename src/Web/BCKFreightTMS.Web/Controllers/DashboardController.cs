namespace BCKFreightTMS.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class DashboardController : BaseController
    {
        private readonly IDeletableEntityRepository<OrderAction> orderActions;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(
            IDeletableEntityRepository<OrderAction> orderActions,
            UserManager<ApplicationUser> userManager)
        {
            this.orderActions = orderActions;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = this.orderActions.All()
                                        .Where(a => a.Until > DateTime.UtcNow && a.Order.Creator.Id == this.userManager.GetUserId(this.User))
                                        .To<ActionIndexViewModel>()
                                        .ToList();
            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [Route("/Error/{StatusCode}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    this.ViewBag.ErrorMessasge = $"Page not found.";
                    break;
                case 500:
                    this.ViewBag.ErrorMessasge = $"Oops! Something went wrong.";
                    break;
                default:
                    this.ViewBag.ErrorMessasge = $"Error";
                    break;
            }

            return this.View(statusCode);
        }
    }
}
