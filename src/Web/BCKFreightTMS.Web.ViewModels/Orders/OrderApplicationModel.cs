namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using BCKFreightTMS.Web.ViewModels.Cargos;
    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class OrderApplicationModel
    {
        public string OrderToReferenceNum { get; set; }

        public string OrderToCompanyName { get; set; }

        public decimal OrderToPriceNetOut { get; set; }

        public string OrderToCurrencyName { get; set; }

        public string OrderToVehicleRegNumber { get; set; }

        public string OrderToVehicleTrailerRegNumber { get; set; }

        public string OrderToCompanyAddressAddressCity { get; set; }

        public string OrderToCompanyAddressAddressStreetLine { get; set; }

        public string OrderToCompanyAddressMOLFirstName { get; set; }

        public string OrderToCompanyAddressMOLLastName { get; set; }

        public string OrderToCompanyTaxNumber { get; set; }

        public string OrderToCompanyComunicatorsMobile1 { get; set; }

        public CompanyViewModel CreatorCompany { get; set; }

        public List<ActionApplicationModel> OrderActions { get; set; }

        public CargoInputModel Cargo { get; set; }

        public DocumentationInputModel Documentation { get; set; }
    }
}
