namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Services.Messaging;
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
        private readonly IDeletableEntityRepository<Currency> currencies;
        private readonly IDeletableEntityRepository<Documentation> documentations;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

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
            IDeletableEntityRepository<VehicleType> vehicleTypes,
            IDeletableEntityRepository<Currency> currencies,
            IDeletableEntityRepository<Documentation> documentations,
            IMapper mapper,
            IEmailSender emailSender)
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
            this.currencies = currencies;
            this.documentations = documentations;
            this.mapper = mapper;
            this.emailSender = emailSender;
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<Order, bool>> whereFilter)
        {
            if (!this.orders.AllAsNoTracking().Any())
            {
                return new List<T>();
            }

            var orders = this.orders.All().Where(whereFilter).To<T>().ToList();
            return orders;
        }

        public OrderAcceptInputModel LoadOrderAcceptInputModel(OrderAcceptInputModel model = null)
        {
            if (model is null)
            {
                model = new OrderAcceptInputModel();
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
            model.CurrencyItems = this.currencies.AllAsNoTracking()
                                                  .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                                  .ToList();
            model.ActionTypeItems = this.actionTypes.AllAsNoTracking()
                                                    .Select(at => new KeyValuePair<string, string>(at.Id.ToString(), at.Name))
                                                    .ToList();
            return model;
        }

        public OrderEditInputModel LoadOrderEditInputModel(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                return null;
            }

            var model = this.mapper.Map<OrderEditInputModel>(order);

            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), $"{c.Name} - {c.TaxNumber}"))
                                               .ToList();
            model.Cargo.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                      .Select(lb => new KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                                      .ToList();
            model.Cargo.TypeItems = this.cargoTypes.AllAsNoTracking()
                                                  .Select(ct => new KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                                  .ToList();
            model.CurrencyItems = this.currencies.AllAsNoTracking()
                                                  .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                                  .ToList();
            model.ActionTypeItems = this.actionTypes.AllAsNoTracking()
                                                    .Select(at => new KeyValuePair<string, string>(at.Id.ToString(), at.Name))
                                                    .ToList();
            return model;
        }

        public OrderCreateInputModel LoadOrderCreateInputModel(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                return null;
            }

            var model = this.mapper.Map<OrderCreateInputModel>(order);

            model.CompanyItems = this.companies.AllAsNoTracking()
                                               .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), $"{c.Name} - {c.TaxNumber}"))
                                               .ToList();
            model.CurrencyItems = this.currencies.AllAsNoTracking()
                                                  .Select(c => new KeyValuePair<string, string>(c.Id.ToString(), c.Name))
                                                  .ToList();
            model.AreasItems = this.orderActions.All()
                                                 .Where(a => (a.Type.Name == ActionTypeNames.Unloading.ToString() ||
                                                             a.Type.Name == ActionTypeNames.Loading.ToString()) &&
                                                                 a.Address.Area != null)
                                                 .Select(a => a.Address.Area)
                                                 .Distinct()
                                                 .Select(a => new SelectListItem { Text = a, Value = a })
                                                 .ToList();
            model.AreasItems = model.AreasItems.Append(new SelectListItem("Any area", string.Empty));

            return model;
        }

        public OrderApplicationModel GenerateApplicationModel(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            var model = this.mapper.Map<OrderApplicationModel>(order);
            if (model.OrderToReferenceNum == null)
            {
                model.OrderToReferenceNum = this.GenerateOrderNumber();
            }

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
                         .Select(v => new SelectListItem
                         {
                             Text = v.Trailer == null ? v.RegNumber : $"{v.RegNumber}/{v.Trailer.RegNumber}",
                             Value = v.Id,
                         })
                         .ToList();
            return vehicles;
        }

        public IEnumerable<SelectListItem> GetCarriersByArea(string area)
        {
            var carriers = this.companies.All();
            if (!string.IsNullOrWhiteSpace(area))
            {
                carriers = carriers.Where(c => c.OrderTos.Any(o =>
                                                            o.Order.OrderActions.Any(oa =>
                                                              oa.Address.Area == area)))
                                                .OrderBy(c => c.OrderTos.Count());
            }

            return carriers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Name} - {c.TaxNumber}" })
                           .ToList();
        }

        public async Task<string> AcceptAsync(OrderAcceptInputModel input, ClaimsPrincipal user)
        {
            var order = new Order
            {
                CreatorId = this.userManager.GetUserId(user),
                Status = this.orderStatuses.All().FirstOrDefault(s => s.Name == OrderStatusNames.Accepted.ToString()),
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

            var documentation = this.mapper.Map<Documentation>(input.Documentation);

            var orderFrom = new OrderFrom
            {
                PriceNetIn = input.PriceNetIn,
                CurrencyId = input.CurrencyInId,
                CompanyId = input.CompanyFromId,
                ContactId = input.ContactFromId,
                TypeId = null,
            };

            await this.orders.AddAsync(order);

            cargo.AdminId = order.AdminId;
            order.Cargo = cargo;
            orderFrom.AdminId = order.AdminId;
            order.OrderFrom = orderFrom;
            documentation.AdminId = order.AdminId;
            order.Documentation = documentation;

            foreach (var actionIM in input.Actions)
            {
                var action = this.mapper.Map<OrderAction>(actionIM);
                action.AdminId = order.AdminId;
                action.Address.AdminId = order.AdminId;
                order.OrderActions.Add(action);
            }

            await this.orders.SaveChangesAsync();
            return order.Id;
        }

        public async Task<string> CreateAsync(OrderCreateInputModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            var orderTo = new OrderTo
            {
                PriceNetOut = input.PriceNetOut,
                CurrencyId = input.CurrencyOutId,
                CompanyId = input.CompanyToId,
                ContactId = input.ContactToId,
                VehicleId = input.VehicleId,
                TypeId = null,
            };

            orderTo.AdminId = order.AdminId;
            orderTo.Drivers.Add(new DriverOrder { OrderId = order.Id, DriverId = input.DriverId, });
            order.OrderTo = orderTo;

            await this.orders.SaveChangesAsync();
            await this.UpdateOrderStatus(order.Id, OrderStatusNames.Ready.ToString());
            return order.Id;
        }

        public async Task<string> EditAsync(OrderEditInputModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);

            var orderTo = order.OrderTo;
            orderTo.CompanyId = input.OrderToCompanyId;
            orderTo.PriceNetOut = input.OrderToPriceNetOut;
            orderTo.CurrencyId = input.OrderToCurrencyId;
            orderTo.ContactId = input.OrderToContactId;
            orderTo.VehicleId = input.OrderToVehicleId;

            order.OrderFrom.ReferenceNum = input.OrderFromReferenceNum;
            order.OrderFrom.PriceNetIn = input.OrderFromPriceNetIn;
            order.OrderFrom.CurrencyId = input.OrderFromCurrencyId;

            var actions = order.OrderActions;
            var notDeletedIds = new List<int>();
            foreach (var inputAction in input.OrderActions)
            {
                var orderAction = actions.FirstOrDefault(a => a.Id == inputAction.Id);
                if (orderAction is null)
                {
                    orderAction = new OrderAction();
                    orderAction.Address = new Address();
                    orderAction.AdminId = order.AdminId;
                    orderAction.Address.AdminId = order.AdminId;
                    actions.Add(orderAction);
                }

                orderAction.TypeId = inputAction.TypeId;
                orderAction.Address.City = inputAction.Address.City;
                orderAction.Address.Postcode = inputAction.Address.Postcode;
                orderAction.Address.Area = inputAction.Address.Area;
                orderAction.Address.State = inputAction.Address.State;
                orderAction.Address.StreetLine = inputAction.Address.StreetLine;
                orderAction.Until = inputAction.Until;
                orderAction.Details = inputAction.Details;
                notDeletedIds.Add(orderAction.Id);
            }
            foreach (var action in actions)
            {
                if (!notDeletedIds.Contains(action.Id))
                {
                    action.IsDeleted = true;
                }
            }

            var cargo = order.Cargo;
            cargo.TypeId = input.Cargo.TypeId;
            cargo.VehicleType = this.vehicleTypes.All().FirstOrDefault(vt => vt.Name == VehicleTypeNames.Truck.ToString());
            cargo.LoadingBodyId = input.Cargo.LoadingBodyId;
            cargo.Name = input.Cargo.Name;
            cargo.Lenght = input.Cargo.Lenght;
            cargo.Width = input.Cargo.Width;
            cargo.Height = input.Cargo.Height;
            cargo.WeightGross = input.Cargo.WeightGross;
            cargo.WeightNet = input.Cargo.WeightNet;
            cargo.Cubature = input.Cargo.Cubature;
            cargo.Quantity = input.Cargo.Quantity;
            cargo.Details = input.Cargo.Details;

            var inputDocumentation = input.Documentation;
            var documentation = order.Documentation;
            documentation.CMR = inputDocumentation.CMR;
            documentation.BillOfLading = inputDocumentation.BillOfLading;
            documentation.AOA = inputDocumentation.AOA;
            documentation.DeliveryNote = inputDocumentation.DeliveryNote;
            documentation.PackingList = inputDocumentation.PackingList;
            documentation.ListItems = inputDocumentation.ListItems;
            documentation.Invoice = inputDocumentation.Invoice;
            documentation.BillOfGoods = inputDocumentation.BillOfGoods;
            await this.documentations.SaveChangesAsync();

            await this.orders.SaveChangesAsync();
            return order.Id;
        }

        public async Task<string> BeginAsync(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);

            order.OrderTo.ReferenceNum = this.GenerateOrderNumber();

            // TODO: Send application
            await this.orders.SaveChangesAsync();
            await this.UpdateOrderStatus(order.Id, OrderStatusNames.InProgress.ToString());
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

        public async Task UpdateOrderStatusAsync(OrderStatusViewModel input)
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

                action.NoNotes = actionInput.NoNotes;
                action.Notes = actionInput.Notes;
                this.orderActions.Update(action);
            }

            // var inputDocumentation = input.Documentation;
            // var documentation = this.documentations.All().FirstOrDefault(d => d.OrderId == input.Id);
            // documentation.CMR = inputDocumentation.CMR;
            // documentation.BillOfLading = inputDocumentation.BillOfLading;
            // documentation.AOA = inputDocumentation.AOA;
            // documentation.DeliveryNote = inputDocumentation.DeliveryNote;
            // documentation.PackingList = inputDocumentation.PackingList;
            // documentation.ListItems = inputDocumentation.ListItems;
            // documentation.Invoice = inputDocumentation.Invoice;
            // documentation.BillOfGoods = inputDocumentation.BillOfGoods;
            // await this.documentations.SaveChangesAsync();
            await this.orderActions.SaveChangesAsync();
        }

        public async Task<string> FinishOrderAsync(OrderFinishViewModel input)
        {
            if (input.OrderStatus.StatusName != OrderStatusNames.Approved.ToString())
            {
                await this.SetOrderReceivedDocumentation(input);
            }

            await this.UpdateOrderStatus(input.OrderStatus.Id, OrderStatusNames.Finished.ToString());
            return input.OrderStatus.Id;
        }

        public bool ValidateFinishModel(OrderFinishViewModel input)
        {
            return !(input.OrderStatus.Documentation.CMR != input.RecievedDocumentation.CMR ||
                     input.OrderStatus.Documentation.AOA != input.RecievedDocumentation.AOA ||
                     input.OrderStatus.Documentation.BillOfLading != input.RecievedDocumentation.BillOfLading ||
                     input.OrderStatus.Documentation.DeliveryNote != input.RecievedDocumentation.DeliveryNote ||
                     input.OrderStatus.Documentation.ListItems != input.RecievedDocumentation.ListItems ||
                     input.OrderStatus.Documentation.PackingList != input.RecievedDocumentation.PackingList ||
                     input.OrderStatus.Documentation.Invoice != input.RecievedDocumentation.Invoice ||
                     input.OrderStatus.Documentation.BillOfGoods != input.RecievedDocumentation.BillOfGoods);
        }

        public OrderFinishViewModel LoadOrderFinishModel(string orderId)
        {
            var model = new OrderFinishViewModel();

            model.OrderStatus = this.LoadOrderStatusModel(orderId);
            var receivedDoc = this.documentations.All().FirstOrDefault(d => d.OrderId == orderId).RecievedDocumentation;
            model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return model;
        }

        public async Task MarkOrderForApproval(OrderFinishViewModel input)
        {
            await this.SetOrderReceivedDocumentation(input);
            await this.UpdateOrderStatus(input.OrderStatus.Id, OrderStatusNames.AwaitingApproval.ToString());
        }

        public async Task ApproveOrder(OrderFinishViewModel input)
        {
            await this.SetOrderReceivedDocumentation(input);
            await this.UpdateOrderStatus(input.OrderStatus.Id, OrderStatusNames.Approved.ToString());
        }

        private string GenerateOrderNumber()
        {
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var ordersCount = this.orders.AllAsNoTracking()
                                         .Where(o =>
                                         o.CreatedOn.Month == month &&
                                         o.Status.Name != OrderStatusNames.Accepted.ToString() &&
                                         o.Status.Name != OrderStatusNames.Ready.ToString())
                                         .Count()
                                         .ToString()
                                         .PadLeft(4, '0');
            return $"{year}{month.ToString().PadLeft(2, '0')}{ordersCount}";
        }

        private async Task UpdateOrderStatus(string orderId, string status)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            order.Status = this.orderStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == status);
            this.orders.Update(order);
            await this.orders.SaveChangesAsync();
        }

        private async Task SetOrderReceivedDocumentation(OrderFinishViewModel input)
        {
            var receivedDoc = this.mapper.Map<Documentation>(input.RecievedDocumentation);
            this.documentations.All().FirstOrDefault(d => d.OrderId == input.OrderStatus.Id).RecievedDocumentation = receivedDoc;
            await this.documentations.SaveChangesAsync();
        }
    }
}
