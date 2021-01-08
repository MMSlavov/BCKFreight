namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class OrderFinishViewModel : IMapFrom<Order>
    {
        public OrderStatusViewModel OrderStatus { get; set; }

        public DocumentationInputModel RecievedDocumentation { get; set; }
    }
}
