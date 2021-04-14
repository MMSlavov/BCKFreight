namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class VATReason : SettingModel
    {
        public VATReason()
        {
            this.InvoiceIns = new HashSet<InvoiceIn>();
            this.InvoiceOuts = new HashSet<InvoiceOut>();
        }

        [Required]
        [MaxLength(200)]
        public override string Name { get; set; }

        public virtual ICollection<InvoiceIn> InvoiceIns { get; set; }

        public virtual ICollection<InvoiceOut> InvoiceOuts { get; set; }
    }
}
