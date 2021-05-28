namespace BCKFreightTMS.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    public class CompanyEditModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        //[MaxLength(90)]
        //public string TaxCountry { get; set; }

        [MaxLength(20)]
        public string TaxNumber { get; set; }

        [MaxLength(100)]
        public string AddressAddressStreetLine { get; set; }

        [MaxLength(20)]
        public string AddressAddressPostcode { get; set; }

        [MaxLength(200)]
        public string AddressAddressCity { get; set; }

        [MaxLength(200)]
        public string AddressAddressState { get; set; }

        [MaxLength(200)]
        public string AddressAddressArea { get; set; }

        [MaxLength(80)]
        public string AddressMOLFirstName { get; set; }

        [MaxLength(80)]
        public string AddressMOLLastName { get; set; }

        [Required]
        [Phone]
        [MaxLength(10)]
        public string ComunicatorsMobile1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string ComunicatorsMobile2 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string ComunicatorsPhone1 { get; set; }

        [Phone]
        [MaxLength(10)]
        public string ComunicatorsPhone2 { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string ComunicatorsEmail1 { get; set; }

        [EmailAddress]
        [MaxLength(10)]
        public string ComunicatorsEmail2 { get; set; }

        public string ComunicatorsDetails { get; set; }
    }
}
