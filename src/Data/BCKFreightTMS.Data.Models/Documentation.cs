namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

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

        public bool WeighingNote { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Problem { get; set; }

        [ForeignKey(nameof(RecievedDocumentation))]
        public int? RecievedDocumentationId { get; set; }

        public virtual Documentation RecievedDocumentation { get; set; }

        public string OrderToId { get; set; }

        public virtual OrderTo Order { get; set; }
    }
}
