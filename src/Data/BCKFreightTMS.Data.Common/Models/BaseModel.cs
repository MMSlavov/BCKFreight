namespace BCKFreightTMS.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseModel<TKey> : IAuditInfo, ICompanyEntity
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string AdminId { get; set; }
    }
}
