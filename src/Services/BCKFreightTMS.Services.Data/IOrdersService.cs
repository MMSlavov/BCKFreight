namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IOrdersService
    {
        public IEnumerable<T> GetAll<T>(Expression<Func<Order, bool>> whereFilter);

        public IEnumerable<T> GetAllOrderTos<T>(Expression<Func<OrderTo, bool>> whereFilter);

        public OrderAcceptInputModel LoadOrderAcceptInputModel(OrderAcceptInputModel model = null);

        public OrderEditInputModel LoadOrderEditInputModel(string orderId);

        public OrderCreateInputModel LoadOrderCreateInputModel(string orderId);

        public OrderFailModel LoadOrderFailModel(string orderId);

        public OrderApplicationModel GenerateApplicationModel(string orderId);

        public CarrierOrderApplicationModel GenerateCarrierApplicationModel(string carrierOrderId);

        public IEnumerable<SelectListItem> GetContacts(string companyId);

        public IEnumerable<SelectListItem> GetDrivers(string companyId);

        public IEnumerable<SelectListItem> GetVehicles(string companyId);

        public IEnumerable<SelectListItem> GetTrailers(string companyId);

        public IEnumerable<SelectListItem> GetCarriersByArea(string area);

        public Task<string> AcceptAsync(OrderAcceptInputModel input, ClaimsPrincipal user);

        public Task<string> CreateAsync(OrderCreateInputModel input);

        public Task<string> EditAsync(OrderEditInputModel input);

        public Task<string> BeginAsync(string orderId);

        public Task<string> ConfirmApplicationAsync(string orderId);

        public Task<string> SetOrderFailAsync(OrderFailModel input);

        public Task<bool> DeleteAsync(string id);

        public OrderStatusViewModel LoadOrderStatusModel(string id);

        public Task FinishOrderToAsync(string orderToId);

        public InvoiceInInputModel LoadOrderFinishModel(string orderId);

        public Task UpdateOrderStatusAsync(OrderStatusViewModel input);

        public Task<string> FinishInvoiceInAsync(InvoiceInInputModel input);

        public bool ValidateFinishModel(InvoiceInInputModel input);

        public Task MarkInvoiceInForApproval(string invoiceId);

        public Task ApproveInvoice(InvoiceInInputModel input);
    }
}
