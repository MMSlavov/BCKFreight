namespace BCKFreightTMS.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CompanyInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(90)]
        public string TaxCountry { get; set; }

        [MaxLength(20)]
        public string TaxNumber { get; set; }

        [MaxLength(100)]
        public string StreetLine { get; set; }

        [MaxLength(20)]
        public string Postcode { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string State { get; set; }

        [MaxLength(200)]
        public string Area { get; set; }

        [MaxLength(80)]
        public string MOLFirstName { get; set; }

        [MaxLength(80)]
        public string MOLLastName { get; set; }

        [Required]
        [Phone]
        [MaxLength(10)]
        public string Mobile1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Mobile2 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Phone1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string Phone2 { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string Email1 { get; set; }

        [EmailAddress]
        [MaxLength(10)]
        public string Email2 { get; set; }

        public string Details { get; set; }
    }
}
