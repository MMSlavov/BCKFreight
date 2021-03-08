namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Invoices;

    public class OrderFinishViewModel : IMapFrom<Order>
    {
        public InvoiceInOrderModel InvoiceInModel { get; set; }

        public OrderStatusViewModel OrderStatus { get; set; }
    }
}
