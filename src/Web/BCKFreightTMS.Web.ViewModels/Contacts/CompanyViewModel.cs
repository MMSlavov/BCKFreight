namespace BCKFreightTMS.Web.ViewModels.Contacts
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CompanyViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TaxCountryName { get; set; }

        public string TaxNumber { get; set; }

        public string AddressAddressStreetLine { get; set; }

        public string AddressMOLFirstName { get; set; }
        
        public string AddressMOLLastName { get; set; }

        public string ComunicatorsMobile1 { get; set; }

        public string ComunicatorsDetails { get; set; }
    }
}
