namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IOrdersService
    {
        public IEnumerable<T> GetAll<T>();

        public OrderInputModel LoadOrderInputModel(OrderInputModel model = null);

        public IEnumerable<SelectListItem> GetContacts(string companyId);

        public IEnumerable<SelectListItem> GetDrivers(string companyId);

        public IEnumerable<SelectListItem> GetVehicles(string companyId);

        public Task<string> CreateAsync(OrderInputModel input, ClaimsPrincipal user);

        public Task<bool> DeleteAsync(string id);

        public OrderStatusViewModel LoadOrderStatusModel(string id);

        public Task UpdateOrderActionsAsync(OrderStatusViewModel input);

        public Task<string> FinishOrderAsync(OrderStatusViewModel input);
    }
}
