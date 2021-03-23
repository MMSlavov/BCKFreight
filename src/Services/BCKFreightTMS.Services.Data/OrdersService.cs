namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Services.Messaging;
    using BCKFreightTMS.Web.ViewModels.Invoices;
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
        private readonly IDeletableEntityRepository<BankDetails> bankDetails;
        private readonly IDeletableEntityRepository<CarrierOrder> carrierOrders;
        private readonly IDeletableEntityRepository<OrderTo> orderTos;
        private readonly IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private readonly IDeletableEntityRepository<InvoiceStatus> invoiceStatuses;
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
            IDeletableEntityRepository<BankDetails> bankDetails,
            IDeletableEntityRepository<CarrierOrder> carrierOrders,
            IDeletableEntityRepository<OrderTo> orderTos,
            IDeletableEntityRepository<InvoiceIn> invoiceIns,
            IDeletableEntityRepository<InvoiceStatus> invoiceStatuses,
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
            this.bankDetails = bankDetails;
            this.carrierOrders = carrierOrders;
            this.orderTos = orderTos;
            this.invoiceIns = invoiceIns;
            this.invoiceStatuses = invoiceStatuses;
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

        public IEnumerable<T> GetAllOrderTos<T>(Expression<Func<OrderTo, bool>> whereFilter)
        {
            if (!this.orders.AllAsNoTracking().Any())
            {
                return new List<T>();
            }

            var orders = this.orderTos.All().Where(whereFilter).To<T>().ToList();
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
            foreach (var orderTo in model.OrderTos)
            {
                orderTo.ContactItems = this.GetContacts(orderTo.CarrierOrderCompanyId);
                orderTo.DriverItems = this.GetDrivers(orderTo.CarrierOrderCompanyId);
                orderTo.VehicleItems = this.GetVehicles(orderTo.CarrierOrderCompanyId);
            }

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
                                                                 a.Address.Area != null &&
                                                                 a.OrderTo.Order.Status.Name == OrderStatusNames.Finished.ToString())
                                                 .Select(a => a.Address.Area)
                                                 .Distinct()
                                                 .Select(a => new SelectListItem { Text = a, Value = a })
                                                 .ToList();
            model.AreasItems = model.AreasItems.Append(new SelectListItem("Any area", string.Empty));

            return model;
        }

        public OrderFailModel LoadOrderFailModel(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                return null;
            }

            var model = this.mapper.Map<OrderFailModel>(order);

            return model;
        }

        public OrderApplicationModel GenerateApplicationModel(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            if (order is null)
            {
                throw new ArgumentException("Company do not exist.");
            }

            var model = this.mapper.Map<OrderApplicationModel>(order);
            var companyTaxCountry = model.CreatorCompany.TaxCountryName;
            foreach (var carrierOrder in model.CarrierOrders)
            {
                if (carrierOrder.ReferenceNum == null)
                {
                    carrierOrder.ReferenceNum = this.GenerateOrderNumber(companyTaxCountry.Equals("bulgaria", StringComparison.InvariantCultureIgnoreCase) ||
                                                                         companyTaxCountry.Equals("българия", StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return model;
        }

        public CarrierOrderApplicationModel GenerateCarrierApplicationModel(string carrierOrderId)
        {
            var order = this.carrierOrders.All().FirstOrDefault(o => o.Id == carrierOrderId);
            if (order is null)
            {
                throw new ArgumentException("Order do not exist.");
            }

            var model = this.mapper.Map<CarrierOrderApplicationModel>(order);

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
                         .Where(v => v.CompanyId == companyId && v.Type.Name != VehicleTypeNames.Trailer.ToString())
                         .Select(v => new SelectListItem
                         {
                             Text = v.Trailer == null ? (v.Type.Name == VehicleTypeNames.Solo.ToString() ? $"{v.RegNumber}(c)" : v.RegNumber) :
                                                        $"{v.RegNumber} / {v.Trailer.RegNumber}",
                             Value = v.Id,
                         })
                         .ToList();
            return vehicles;
        }

        public IEnumerable<SelectListItem> GetTrailers(string companyId)
        {
            var vehicles = this.vehicles.AllAsNoTracking()
                         .Where(v => v.CompanyId == companyId && v.Type.Name == VehicleTypeNames.Trailer.ToString())
                         .Select(v => new SelectListItem
                         {
                             Text = v.RegNumber,
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
                                                            o.Order.OrderTos.SelectMany(o => o.OrderActions).Any(oa =>
                                                              oa.Address.Area == area)))
                                                .OrderByDescending(c => c.OrderTos.Count());
            }

            return carriers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.Name} - {c.TaxNumber}" })
                           .ToList();
        }

        public async Task<string> AcceptAsync(OrderAcceptInputModel input, ClaimsPrincipal user)
        {
            var order = new Order
            {
                CreatorId = this.userManager.GetUserId(user),
                DueDaysFrom = input.DueDaysFrom,
                Status = this.orderStatuses.All().FirstOrDefault(s => s.Name == OrderStatusNames.Accepted.ToString()),
                OrderTos = new List<OrderTo>(),
            };

            var orderFrom = new OrderFrom
            {
                CompanyId = input.CompanyFromId,
                ContactId = input.ContactFromId,
                TypeId = null,
            };

            await this.orders.AddAsync(order);

            foreach (var orderToInput in input.OrderTos)
            {
                var orderTo = new OrderTo
                {
                    PriceNetIn = orderToInput.PriceNetIn,
                    CurrencyInId = orderToInput.CurrencyInId,
                    PriceNetOut = orderToInput.PriceNetOut,
                    CurrencyOutId = orderToInput.CurrencyOutId,
                    TypeId = null,
                };

                var cargo = new Cargo
                {
                    TypeId = orderToInput.Cargo.TypeId,
                    VehicleType = this.vehicleTypes.All().FirstOrDefault(vt => vt.Name == VehicleTypeNames.Truck.ToString()),
                    LoadingBodyId = orderToInput.Cargo.LoadingBodyId,
                    VehicleRequirements = orderToInput.Cargo.VehicleRequirements,
                    Name = orderToInput.Cargo.Name,
                    Lenght = orderToInput.Cargo.Lenght,
                    Width = orderToInput.Cargo.Width,
                    Height = orderToInput.Cargo.Height,
                    WeightGross = orderToInput.Cargo.WeightGross,
                    WeightNet = orderToInput.Cargo.WeightNet,
                    Cubature = orderToInput.Cargo.Cubature,
                    Quantity = orderToInput.Cargo.Quantity,
                    Details = orderToInput.Cargo.Details,
                };

                var documentation = this.mapper.Map<Documentation>(orderToInput.Documentation);
                documentation.RecievedDocumentation = new Documentation();
                documentation.OrderToId = orderTo.Id;

                orderTo.AdminId = order.AdminId;
                cargo.AdminId = order.AdminId;
                orderTo.Cargo = cargo;
                documentation.AdminId = order.AdminId;
                documentation.RecievedDocumentation.AdminId = order.AdminId;
                orderTo.Documentation = documentation;

                var actions = orderToInput.OrderActions.OrderBy(a => a.TypeId);
                foreach (var actionIM in actions)
                {
                    var action = this.mapper.Map<OrderAction>(actionIM);
                    action.AdminId = order.AdminId;
                    action.Address.AdminId = order.AdminId;
                    orderTo.OrderActions.Add(action);
                }

                order.OrderTos.Add(orderTo);
            }

            orderFrom.AdminId = order.AdminId;
            order.OrderFrom = orderFrom;

            await this.orders.SaveChangesAsync();
            return order.Id;
        }

        public async Task<string> CreateAsync(OrderCreateInputModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            order.DueDaysTo = input.DueDaysTo;
            input.OrderTos.Select(ot => ot.CarrierOrderCompanyId)
                            .Distinct()
                            .ToList()
                            .ForEach(id =>
                            {
                                order.CarrierOrders.Add(new CarrierOrder { CompanyId = id, OrderTos = new List<OrderTo>() });
                            });

            foreach (var orderToInput in input.OrderTos)
            {
                var vehicle = this.vehicles.All().FirstOrDefault(v => v.Id == orderToInput.VehicleId);
                if (!string.IsNullOrWhiteSpace(orderToInput.VehicleTrailerId))
                {
                    vehicle.TrailerId = orderToInput.VehicleTrailerId;
                    await this.vehicles.SaveChangesAsync();
                }

                var orderTo = order.OrderTos.FirstOrDefault(o => o.Id == orderToInput.Id);
                orderTo.PriceNetOut = orderToInput.PriceNetOut;
                orderTo.CurrencyOutId = orderToInput.CurrencyOutId;
                orderTo.ContactId = orderToInput.ContactId;
                orderTo.VehicleId = orderToInput.VehicleId;
                orderTo.TypeId = null;

                orderTo.AdminId = order.AdminId;
                orderTo.Drivers.Add(new DriverOrder { OrderId = orderTo.Id, DriverId = orderToInput.DriverId, });
                var carrierOrder = order.CarrierOrders.FirstOrDefault(co => co.CompanyId == orderToInput.CarrierOrderCompanyId);
                carrierOrder.OrderTos.Add(orderTo);
                await this.carrierOrders.SaveChangesAsync();
                carrierOrder.AdminId = order.AdminId;
            }

            await this.orders.SaveChangesAsync();
            await this.UpdateOrderStatus(order.Id, OrderStatusNames.Ready.ToString());
            return order.Id;
        }

        public async Task<string> EditAsync(OrderEditInputModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);

            order.OrderFrom.ReferenceNum = input.OrderFromReferenceNum;

            foreach (var orderTo in order.OrderTos)
            {
                var orderToInput = input.OrderTos.FirstOrDefault(o => o.Id == orderTo.Id);
                orderTo.CarrierOrder.CompanyId = orderToInput.CarrierOrderCompanyId;
                orderTo.PriceNetOut = orderToInput.PriceNetOut;
                orderTo.CurrencyOutId = orderToInput.CurrencyOutId;
                orderTo.PriceNetIn = orderToInput.PriceNetIn;
                orderTo.CurrencyInId = orderToInput.CurrencyInId;
                orderTo.ContactId = orderToInput.ContactId;
                orderTo.VehicleId = orderToInput.VehicleId;

                var actions = orderTo.OrderActions;
                var notDeletedIds = new List<int>();
                foreach (var inputAction in orderToInput.OrderActions)
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

                var cargo = orderTo.Cargo;
                cargo.TypeId = orderToInput.Cargo.TypeId;
                cargo.VehicleType = this.vehicleTypes.All().FirstOrDefault(vt => vt.Name == VehicleTypeNames.Truck.ToString());
                cargo.LoadingBodyId = orderToInput.Cargo.LoadingBodyId;
                cargo.Name = orderToInput.Cargo.Name;
                cargo.Lenght = orderToInput.Cargo.Lenght;
                cargo.Width = orderToInput.Cargo.Width;
                cargo.Height = orderToInput.Cargo.Height;
                cargo.WeightGross = orderToInput.Cargo.WeightGross;
                cargo.WeightNet = orderToInput.Cargo.WeightNet;
                cargo.Cubature = orderToInput.Cargo.Cubature;
                cargo.Quantity = orderToInput.Cargo.Quantity;
                cargo.Details = orderToInput.Cargo.Details;

                var inputDocumentation = orderToInput.Documentation;
                var documentation = orderTo.Documentation;
                documentation.CMR = inputDocumentation.CMR;
                documentation.BillOfLading = inputDocumentation.BillOfLading;
                documentation.AOA = inputDocumentation.AOA;
                documentation.DeliveryNote = inputDocumentation.DeliveryNote;
                documentation.PackingList = inputDocumentation.PackingList;
                documentation.ListItems = inputDocumentation.ListItems;
                documentation.Invoice = inputDocumentation.Invoice;
                documentation.BillOfGoods = inputDocumentation.BillOfGoods;
            }

            await this.documentations.SaveChangesAsync();
            await this.orders.SaveChangesAsync();
            return order.Id;
        }

        public async Task<string> BeginAsync(string orderId)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            var companyTaxCountry = order.Creator.Company.TaxCountry.Name;
            foreach (var carrierOrder in order.CarrierOrders)
            {
                carrierOrder.ReferenceNum = this.GenerateOrderNumber(companyTaxCountry.Equals("bulgaria", StringComparison.InvariantCultureIgnoreCase) ||
                                                            companyTaxCountry.Equals("българия", StringComparison.InvariantCultureIgnoreCase));
                await this.carrierOrders.SaveChangesAsync();
            }

            // TODO: Send application
            await this.orders.SaveChangesAsync();
            await this.UpdateOrderStatus(order.Id, OrderStatusNames.AwaitingApplication.ToString());
            return order.Id;
        }

        public async Task<string> ConfirmApplicationAsync(string orderId)
        {
            await this.UpdateOrderStatus(orderId, OrderStatusNames.InProgress.ToString());
            return orderId;
        }

        public async Task<string> SetOrderFailAsync(OrderFailModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            if (order is null)
            {
                return null;
            }

            order.FailReason = input.FailReason;

            await this.UpdateOrderStatus(input.Id, OrderStatusNames.Fail.ToString());
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
                foreach (var action in this.orderActions.All().Where(a => a.OrderTo.Order.Id == id))
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
            var order = this.orders.All().FirstOrDefault(o => o.Id == id);
            var model = this.mapper.Map<OrderStatusViewModel>(order);
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

            // foreach (var orderTo in model.OrderTos)
            // {
            //    orderTo.OrderStatusActions = this.orderActions.All()
            //                                     .Where(oa => oa.OrderToId == orderTo.Id)
            //                                     .OrderBy(oa => oa.TypeId)
            //                                     .To<ActionStatusInputModel>()
            //                                     .ToList();
            // }
            return model;
        }

        public async Task UpdateOrderStatusAsync(OrderStatusViewModel input)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == input.Id);
            foreach (var orderToInput in input.OrderTos)
            {
                var orderTo = order.OrderTos.FirstOrDefault(ot => ot.Id == orderToInput.Id);
                foreach (var actionInput in orderToInput.OrderStatusActions)
                {
                    var action = orderTo.OrderActions.FirstOrDefault(oa => oa.Id == actionInput.Id);
                    action.NotFinishedReasonId = actionInput.NotFinishedReasonId == 0 ? null : actionInput.NotFinishedReasonId;
                    action.NoNotes = actionInput.NoNotes;
                    action.Notes = actionInput.Notes;
                    if (action.NoNotes || !string.IsNullOrWhiteSpace(actionInput.Notes))
                    {
                        action.IsFinished = true;
                    }

                    this.orderActions.Update(action);
                }
            }

            if (order.OrderTos.All(oa => oa.IsFinished))
            {
                order.Status = this.orderStatuses.AllAsNoTracking()
                                                    .FirstOrDefault(s => s.Name == OrderStatusNames.Finished.ToString());
            }

            await this.orderTos.SaveChangesAsync();
        }

        public async Task FinishOrderToAsync(string orderToId)
        {
            var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderToId);

            if (!orderTo.OrderActions.All(a => a.IsFinished))
            {
                throw new InvalidOperationException("Not all actions are finished!");
            }

            orderTo.IsFinished = true;

            await this.orderTos.SaveChangesAsync();
        }

        public async Task<string> FinishInvoiceInAsync(InvoiceInInputModel input)
        {
            await this.SetOrderReceivedDocumentation(input);

            var invoiceIn = new InvoiceIn();
            invoiceIn = this.mapper.Map<InvoiceInModel, InvoiceIn>(input.InvoiceIn, invoiceIn);
            foreach (var orderToInput in input.OrderTos)
            {
                var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderToInput.Id);
                orderTo.InvoiceIn = invoiceIn;
            }

            await this.invoiceIns.AddAsync(invoiceIn);
            await this.invoiceIns.SaveChangesAsync();
            return invoiceIn.Id;
        }

        public bool ValidateFinishModel(InvoiceInInputModel input)
        {
            foreach (var orderTo in input.OrderTos)
            {
                if (orderTo.Documentation.CMR != orderTo.DocumentationRecievedDocumentation.CMR ||
                    orderTo.Documentation.AOA != orderTo.DocumentationRecievedDocumentation.AOA ||
                    orderTo.Documentation.BillOfLading != orderTo.DocumentationRecievedDocumentation.BillOfLading ||
                    orderTo.Documentation.DeliveryNote != orderTo.DocumentationRecievedDocumentation.DeliveryNote ||
                    orderTo.Documentation.ListItems != orderTo.DocumentationRecievedDocumentation.ListItems ||
                    orderTo.Documentation.PackingList != orderTo.DocumentationRecievedDocumentation.PackingList ||
                    orderTo.Documentation.Invoice != orderTo.DocumentationRecievedDocumentation.Invoice ||
                    orderTo.Documentation.BillOfGoods != orderTo.DocumentationRecievedDocumentation.BillOfGoods ||
                    orderTo.Documentation.WeighingNote != orderTo.DocumentationRecievedDocumentation.WeighingNote)
                {
                    return false;
                }
            }

            return true;
        }

        public InvoiceInInputModel LoadOrderFinishModel(string orderId)
        {
            var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderId);

            var invoiceInModel = this.mapper.Map<InvoiceInInputModel>(orderTo);
            invoiceInModel.InvoiceIn = new InvoiceInModel();
            invoiceInModel.InvoiceIn.BankDetailsItems = orderTo.CarrierOrder.Company.BankDetails
                                                               .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceInModel.InvoiceIn.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();
            invoiceInModel.OrderTos = new List<OrderToInvoiceModel>();
            invoiceInModel.OrderTos.Add(this.mapper.Map<OrderToInvoiceModel>(orderTo));

            // var receivedDoc = order.Documentation.RecievedDocumentation;
            // model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return invoiceInModel;
        }

        public async Task MarkInvoiceInForApproval(string invoiceId)
        {
            await this.UpdateInvoiceInStatus(invoiceId, OrderStatusNames.AwaitingApproval.ToString());
        }

        public async Task ApproveInvoice(InvoiceInInputModel input)
        {
            await this.SetOrderReceivedDocumentation(input);
            await this.UpdateInvoiceInStatus(input.InvoiceIn.Id, OrderStatusNames.Approved.ToString());
        }

        private string GenerateOrderNumber(bool isBG)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString().PadLeft(2, '0');
            var ordersCount = this.carrierOrders.AllAsNoTracking()
                                         .Where(o => o.ReferenceNum != null)
                                         .ToList()
                                         .Where(co => this.IsNumberInSameMonth(co.ReferenceNum, month, isBG))
                                         .Count() + 1;
            var number = ordersCount.ToString().PadLeft(4, '0');
            if (isBG)
            {
                return string.Format(GlobalConstants.BGOrderNumberFormat, year, month, number);
            }
            else
            {
                return string.Format(GlobalConstants.NonBGOrderNumberFormat, year, month, number);
            }
        }

        private bool IsNumberInSameMonth(string number, string month, bool isBG)
        {
            if (isBG)
            {
                return number.Substring(4, 2) == month;
            }
            else
            {
                return number.Substring(0, 2) == month;
            }
        }

        private async Task UpdateOrderStatus(string orderId, string status)
        {
            var order = this.orders.All().FirstOrDefault(o => o.Id == orderId);
            order.Status = this.orderStatuses.AllAsNoTracking()
                                             .FirstOrDefault(s => s.Name == status);
            this.orders.Update(order);
            await this.orders.SaveChangesAsync();
        }

        private async Task UpdateInvoiceInStatus(string invoiceId, string status)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(o => o.Id == invoiceId);
            invoice.Status = this.invoiceStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == status);
            this.invoiceIns.Update(invoice);
            await this.invoiceIns.SaveChangesAsync();
        }

        private async Task SetOrderReceivedDocumentation(InvoiceInInputModel input)
        {
            foreach (var orderTo in input.OrderTos)
            {
                var receivedDoc = this.mapper.Map<Documentation>(orderTo.DocumentationRecievedDocumentation);
                this.documentations.All().FirstOrDefault(d => d.OrderToId == orderTo.Id).RecievedDocumentation = receivedDoc;
            }

            await this.documentations.SaveChangesAsync();
        }
    }
}
