namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OrdersService : IOrdersService
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
        private readonly IDeletableEntityRepository<VehicleType> vehicleTypes;

        public OrdersService(
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
            IDeletableEntityRepository<OrderStatus> orderStatuses,
            IDeletableEntityRepository<VehicleType> vehicleTypes)
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
            this.vehicleTypes = vehicleTypes;
        }

        public IEnumerable<T> GetAll<T>()
        {
            if (!this.orders.AllAsNoTracking().Any())
            {
                return null;
            }

            var orders = this.orders.All().To<T>().ToList();
            return orders;
        }

        public OrderInputModel LoadOrderInputModel(OrderInputModel model = null)
        {
            if (model is null)
            {
                model = new OrderInputModel();
            }

            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), $"{c.Name} - {c.TaxNumber}"))
                                               .ToList();
            model.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                      .Select(lb => new KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                                      .ToList();
            model.CargoTypeItems = this.cargoTypes.AllAsNoTracking()
                                          .Select(ct => new KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                          .ToList();
            return model;
        }

        public IEnumerable<SelectListItem> GetContacts(string companyId)
        {
            var contacts = this.people.AllAsNoTracking()
                         .Where(p => p.CompanyId == companyId & p.Role.Name == PersonRoleNames.Contact.ToString())
                         .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                         .ToList();
            return contacts;
        }

        public IEnumerable<SelectListItem> GetDrivers(string companyId)
        {
            var drivers = this.people.AllAsNoTracking()
                         .Where(p => p.CompanyId == companyId & p.Role.Name == PersonRoleNames.Driver.ToString())
                         .Select(p => new SelectListItem { Text = p.FirstName + " " + p.LastName, Value = p.Id })
                         .ToList();
            return drivers;
        }

        public IEnumerable<SelectListItem> GetVehicles(string companyId)
        {
            var vehicles = this.vehicles.AllAsNoTracking()
                         .Where(v => v.CompanyId == companyId)
                         .Select(v => new SelectListItem { Text = v.RegNumber, Value = v.Id })
                         .ToList();
            return vehicles;
        }

        public async Task<string> CreateAsync(OrderInputModel input, ClaimsPrincipal user)
        {
            var order = new Order
            {
                CreatorId = this.userManager.GetUserId(user),
                Status = this.orderStatuses.All().FirstOrDefault(s => s.Name == OrderStatusNames.InProgress.ToString()),
            };

            var cargo = new Cargo
            {
                TypeId = input.CargoTypeId,
                VehicleType = this.vehicleTypes.All().FirstOrDefault(vt => vt.Name == VehicleTypeNames.Truck.ToString()),
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
                TypeId = null,
            };

            var orderTo = new OrderTo
            {
                PriceNetOut = input.PriceNetOut,
                CompanyId = input.CompanyToId,
                ContactId = input.ContactToId,
                VehicleId = input.VehicleId,
                TypeId = null,
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
                Type = this.actionTypes.All().FirstOrDefault(t => t.Name == ActionTypeNames.Loading.ToString()),
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
                Type = this.actionTypes.All().FirstOrDefault(t => t.Name == ActionTypeNames.Unloading.ToString()),
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
            return order.Id;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == id);
            if (order is null)
            {
                return false;
            }
            else
            {
                this.orders.Delete(order);
                foreach (var action in this.orderActions.All().Where(a => a.OrderId == id))
                {
                    this.orderActions.Delete(action);
                }

                await this.orderActions.SaveChangesAsync();
                await this.orders.SaveChangesAsync();
            }

            return true;
        }

        public OrderStatusViewModel LoadOrderStatusModel(string id)
        {
            var model = this.orders.All().Where(x => x.Id == id).To<OrderStatusViewModel>().FirstOrDefault();
            if (model is null)
            {
                return null;
            }

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

        public async Task UpdateOrderActionsAsync(OrderStatusViewModel input)
        {
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
        }

        public async Task<string> FinishOrderAsync(OrderStatusViewModel input)
        {
            await this.UpdateOrderActionsAsync(input);
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            order.StatusId = this.orderStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == OrderStatusNames.Finished.ToString())
                                               .Id;
            this.orders.Update(order);
            await this.orders.SaveChangesAsync();
            return order.Id;
        }
    }
}
