namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class DocumentationInputModel : IMapFrom<Documentation>
    {
        public bool CMR { get; set; }

        public bool BillOfLading { get; set; }

        public bool AOA { get; set; }

        public bool DeliveryNote { get; set; }

        public bool PackingList { get; set; }

        public bool ListItems { get; set; }

        public bool Invoice { get; set; }

        public bool BillOfGoods { get; set; }

        public bool WeighingNote { get; set; }

        public string Problem { get; set; }
    }
}
