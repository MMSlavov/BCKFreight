namespace BCKFreightTMS.Web.ViewModels.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Web.ViewModels.Comunicators;

    public class PersonInputModel
    {
        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        public ComunicatorsInputModel Comunicators { get; set; }

        public string CompanyId { get; set; }

        public int RoleId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> RoleItems { get; set; }
    }
}
