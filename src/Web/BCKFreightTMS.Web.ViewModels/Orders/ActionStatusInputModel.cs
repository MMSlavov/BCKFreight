namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ActionStatusInputModel : IMapFrom<OrderAction>
    {
        public int Id { get; set; }

        public bool IsFinnished { get; set; }

        public int NotFinishedReasonId { get; set; }

        public string Details { get; set; }

        public string AddressCity { get; set; }

        public string AddressStreetLine { get; set; }

        public int TypeId { get; set; }
    }
}
