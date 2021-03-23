namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class OrderTo : BaseDeletableModel<string>
    {
        public OrderTo()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Drivers = new HashSet<DriverOrder>();
            this.OrderActions = new HashSet<OrderAction>();
        }

        [ForeignKey(nameof(CarrierOrder))]
        public string CarrierOrderId { get; set; }

        public virtual CarrierOrder CarrierOrder { get; set; }

        public decimal PriceNetOut { get; set; }

        [ForeignKey(nameof(CurrencyOut))]
        public int? CurrencyOutId { get; set; }

        public virtual Currency CurrencyOut { get; set; }

        public decimal PriceNetIn { get; set; }

        [ForeignKey(nameof(CurrencyIn))]
        public int? CurrencyInId { get; set; }

        public virtual Currency CurrencyIn { get; set; }

        [ForeignKey(nameof(Vehicle))]
        public string VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        [ForeignKey(nameof(Type))]
        public int? TypeId { get; set; }

        public virtual OrderType Type { get; set; }

        public string CargoId { get; set; }

        public virtual Cargo Cargo { get; set; }

        public int? DocumentationId { get; set; }

        [ForeignKey(nameof(DocumentationId))]
        public virtual Documentation Documentation { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string FailReason { get; set; }

        public string ContactId { get; set; }

        public virtual Person Contact { get; set; }

        public virtual Order Order { get; set; }

        public bool IsFinished { get; set; }

        [ForeignKey(nameof(InvoiceIn))]
        public string InvoiceInId { get; set; }

        public virtual InvoiceIn InvoiceIn { get; set; }

        public virtual ICollection<DriverOrder> Drivers { get; set; }

        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
