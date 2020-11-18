﻿namespace BCKFreightTMS.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BCKFreightTMS.Data.Common.Models;

    public class Person : BaseDeletableModel<string>
    {
        public Person()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ContactOrdersTo = new HashSet<OrderTo>();
            this.ContactOrdersFrom = new HashSet<OrderFrom>();
            this.DriverOrders = new HashSet<OrderTo>();
        }

        [ForeignKey(nameof(Company))]
        public string CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public PersonRole Role { get; set; }

        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public int ComunicatorsId { get; set; }

        public Comunicators Comunicators { get; set; }

        public virtual ICollection<OrderTo> ContactOrdersTo { get; set; }

        public virtual ICollection<OrderFrom> ContactOrdersFrom { get; set; }

        public virtual ICollection<OrderTo> DriverOrders { get; set; }
    }
}