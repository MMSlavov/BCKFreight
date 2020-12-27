namespace BCKFreightTMS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize(Roles = "User")]
    public class OrdersController : BaseController
    {
        private readonly IDeletableEntityRepository<Company> companies;
        private readonly IDeletableEntityRepository<Order> orders;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadingBodies;
        private readonly IDeletableEntityRepository<CargoType> cargoTypes;
        private readonly IDeletableEntityRepository<Person> people;
        private readonly IDeletableEntityRepository<Vehicle> vehicles;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ActionNotFinishedReason> actionNFReasons;
        private readonly IDeletableEntityRepository<OrderAction> orderActions;
        private readonly IDeletableEntityRepository<ActionType> actionTypes;
        private readonly IDeletableEntityRepository<OrderStatus> orderStatuses;

        public OrdersController(
            IDeletableEntityRepository<Company> companies,
            IDeletableEntityRepository<Order> orders,
            IDeletableEntityRepository<VehicleLoadingBody> loadingBodies,
            IDeletableEntityRepository<CargoType> cargoTypes,
            IDeletableEntityRepository<Person> people,
            IDeletableEntityRepository<Vehicle> vehicles,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ActionNotFinishedReason> actionNFReasons,
            IDeletableEntityRepository<OrderAction> orderActions,
            IDeletableEntityRepository<ActionType> actionTypes,
            IDeletableEntityRepository<OrderStatus> orderStatuses)
        {
            this.companies = companies;
            this.orders = orders;
            this.loadingBodies = loadingBodies;
            this.cargoTypes = cargoTypes;
            this.people = people;
            this.vehicles = vehicles;
            this.userManager = userManager;
            this.actionNFReasons = actionNFReasons;
            this.orderActions = orderActions;
            this.actionTypes = actionTypes;
            this.orderStatuses = orderStatuses;
        }

        public IActionResult Index()
        {
            var orders = this.orders.All().To<ListOrderViewModel>().ToList();
            return this.View(orders);
        }

        public IActionResult Create()
        {
            var model = new OrderInputModel();
            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), $"{c.Name} - {c.TaxNumber}"))
                                               .ToList();
            model.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                      .Select(lb => new KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                                      .ToList();
            model.CargoTypeItems = this.cargoTypes.AllAsNoTracking()
                                          .Select(ct => new KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                          .ToList();
            return this.View(model);
        }

        public JsonResult GetContacts(string companyId)
        {
            var contacts = this.people.AllAsNoTracking()
                         .Where(p => p.CompanyId == companyId & p.Role.Name == PersonRoleNames.Contact.ToString())
                         .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                         .ToList();
            return this.Json(contacts);
        }

        public JsonResult GetDrivers(string companyId)
        {
            var drivers = this.people.AllAsNoTracking()
                         .Where(p => p.CompanyId == companyId & p.Role.Name == PersonRoleNames.Driver.ToString())
                         .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                         .ToList();
            return this.Json(drivers);
        }

        public JsonResult GetVehicles(string companyId)
        {
            var vehicles = this.vehicles.AllAsNoTracking()
                         .Where(v => v.CompanyId == companyId)
                         .Select(v => new SelectListItem { Text = v.RegNumber, Value = v.Id })
                         .ToList();
            return this.Json(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CompanyItems = this.companies.AllAsNoTracking()
                                                   .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), $"{c.Name} - {c.TaxNumber}"))
                                                   .ToList();
                input.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                          .Select(vt => new KeyValuePair<string, string>(vt.Id.ToString(), vt.Name))
                                                          .ToList();
                input.CargoTypeItems = this.cargoTypes.AllAsNoTracking()
                                                      .Select(ct => new KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                                      .ToList();
                return this.View(input);
            }

            var order = new Order
            {
                CreatorId = this.userManager.GetUserId(this.User),
                StatusId = 2,
            };

            var cargo = new Cargo
            {
                TypeId = input.CargoTypeId,
                VehicleTypeId = 1,
                LoadingBodyId = input.LoadingBodyId,
                Name = input.CargoName,
                Lenght = input.Lenght,
                Width = input.Width,
                Height = input.Height,
                WeightGross = input.WeightGross,
                WeightNet = input.WeightNet,
                Cubature = input.Cubature,
                Quantity = input.Quantity,
                Details = input.Details,
            };

            var orderFrom = new OrderFrom
            {
                PriceNetIn = input.PriceNetIn,
                CompanyId = input.CompanyFromId,
                ContactId = input.ContactFromId,
                TypeId = 1,
            };

            var orderTo = new OrderTo
            {
                PriceNetOut = input.PriceNetOut,
                CompanyId = input.CompanyToId,
                ContactId = input.ContactToId,
                VehicleId = input.VehicleId,
                TypeId = 1,
            };

            var loadingAction = new OrderAction
            {
                Until = input.LoadingUntil.ToUniversalTime(),
                Details = input.LoadingDetails,
                Address = new Address
                {
                    City = input.LoadingCity,
                    Postcode = input.LoadingPostcode,
                    State = input.LoadingState,
                    Area = input.LoadingArea,
                    StreetLine = input.LoadingStreetLine,
                },
                TypeId = 1,
            };
            var unloadingAction = new OrderAction
            {
                Until = input.UnloadingUntil.ToUniversalTime(),
                Details = input.UnloadingDetails,
                Address = new Address
                {
                    City = input.UnloadingCity,
                    Postcode = input.UnloadingPostcode,
                    State = input.UnloadingState,
                    Area = input.UnloadingArea,
                    StreetLine = input.UnloadingStreetLine,
                },
                TypeId = 2,
            };

            await this.orders.AddAsync(order);

            cargo.AdminId = order.AdminId;
            order.Cargo = cargo;
            orderFrom.AdminId = order.AdminId;
            order.OrderFrom = orderFrom;
            orderTo.AdminId = order.AdminId;
            orderTo.Drivers.Add(new DriverOrder { OrderId = order.Id, DriverId = input.DriverId, });
            order.OrderTo = orderTo;
            loadingAction.AdminId = order.AdminId;
            loadingAction.Address.AdminId = order.AdminId;
            order.OrderActions.Add(loadingAction);
            unloadingAction.AdminId = order.AdminId;
            unloadingAction.Address.AdminId = order.AdminId;
            order.OrderActions.Add(unloadingAction);

            await this.orders.SaveChangesAsync();
            return this.RedirectToAction(GlobalConstants.Index);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == id);
            if (order is null)
            {
                this.ModelState.AddModelError(string.Empty, "Order do not exist.");
            }
            else
            {
                this.orders.Delete(order);
                foreach (var action in this.orderActions.All().Where(a => a.OrderId == id))
                {
                    this.orderActions.Delete(action);
                }

                await this.orders.SaveChangesAsync();
            }

            return this.RedirectToAction(GlobalConstants.Index);
        }

        public IActionResult Status(string id)
        {
            if (this.orders.All().FirstOrDefault(o => o.Id == id).Status.Name == OrderStatusNames.Finished.ToString())
            {
                return this.RedirectToAction(GlobalConstants.Index);
            }

            var model = this.LoadOrderStatusModel(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Status(OrderStatusViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(this.LoadOrderStatusModel(input.Id));
            }

            foreach (var actionInput in input.Actions)
            {
                var action = this.orderActions.All().FirstOrDefault(oa => oa.Id == actionInput.Id);
                action.IsFinished = actionInput.IsFinnished;
                if (actionInput.NotFinishedReasonId == 0)
                {
                    action.NotFinishedReason = null;
                }
                else
                {
                    action.NotFinishedReasonId = actionInput.NotFinishedReasonId;
                }

                this.orderActions.Update(action);
            }

            await this.orderActions.SaveChangesAsync();
            return this.RedirectToAction(GlobalConstants.Index);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(OrderStatusViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Status", this.LoadOrderStatusModel(input.Id));
            }

            if (input.Actions.Any(a => a.IsFinnished == false))
            {
                this.ModelState.AddModelError(string.Empty, "All actions must be completed to finish order.");
                return this.RedirectToAction("Status", this.LoadOrderStatusModel(input.Id));
            }

            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            order.StatusId = this.orderStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == OrderStatusNames.Finished.ToString())
                                               .Id;
            this.orders.Update(order);
            await this.orders.SaveChangesAsync();
            return this.RedirectToAction(GlobalConstants.Index);
        }

        private OrderStatusViewModel LoadOrderStatusModel(string id)
        {
            var model = this.orders.All().Where(x => x.Id == id).To<OrderStatusViewModel>().FirstOrDefault();
            model.ActionNotFinishedItems = this.actionNFReasons.AllAsNoTracking()
                           .Select(ar => new System.Collections.Generic.KeyValuePair<string, string>(ar.Id.ToString(), ar.Name))
                           .ToList();
            model.ActionTypeItems = this.actionTypes.AllAsNoTracking()
               .Select(ar => new System.Collections.Generic.KeyValuePair<string, string>(ar.Id.ToString(), ar.Name))
               .ToList();
            model.Actions = this.orderActions.All()
                                             .Where(oa => oa.OrderId == model.Id)
                                             .To<ActionStatusInputModel>()
                                             .ToList();
            return model;
        }
    }
}
