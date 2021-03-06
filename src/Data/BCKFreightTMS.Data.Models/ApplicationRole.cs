﻿// ReSharper disable VirtualMemberCallInConstructor
namespace BCKFreightTMS.Data.Models
{
    using System;

    using BCKFreightTMS.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity, ICompanyEntity
    {
        public ApplicationRole()
            : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string AdminId { get; set; }
    }
}
