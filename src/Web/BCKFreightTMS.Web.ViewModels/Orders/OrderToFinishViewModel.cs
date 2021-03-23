namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Invoices;

    public class OrderToFinishViewModel : IMapFrom<OrderTo>
    {
        public InvoiceInInputModel InvoiceInModel { get; set; }

        public List<OrderToModel> OrderTos { get; set; }

        public string OrderStatusName { get; set; }

        public string Id { get; set; }
    }
}
