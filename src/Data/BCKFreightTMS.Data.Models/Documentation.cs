namespace BCKFreightTMS.Data.Models
{
    using BCKFreightTMS.Data.Common.Models;

    public class Documentation : BaseDeletableModel<int>
    {
        public bool CMR { get; set; }

        public bool BillOfLading { get; set; }

        public bool AOA { get; set; }

        public bool DeliveryNote { get; set; }

        public bool PackingList { get; set; }

        public bool ListItems { get; set; }

        public bool Invoice { get; set; }

        public bool BillOfGoods { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
