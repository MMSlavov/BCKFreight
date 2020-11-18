namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class DriverOrder : IAuditInfo, IDeletableEntity
    {
        [Required]
        public string DriverId { get; set; }

        public Person Driver { get; set; }

        [Required]
        public string OrderId { get; set; }

        public OrderTo Order { get; set; }

        public DateTime CreatedOn { get ; set ; }

        public DateTime? ModifiedOn { get ; set; }

        public bool IsDeleted { get ; set ; }

        public DateTime? DeletedOn { get ; set ; }
    }
}
