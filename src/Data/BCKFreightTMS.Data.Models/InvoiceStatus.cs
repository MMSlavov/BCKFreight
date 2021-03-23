namespace BCKFreightTMS.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class InvoiceStatus : BaseDeletableModel<int>
    {
        public InvoiceStatus()
        {
            this.InvoiceIns = new HashSet<InvoiceIn>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<InvoiceIn> InvoiceIns { get; set; }
    }
}
