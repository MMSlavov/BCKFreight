namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Web.ViewModels.Invoices;

    public class OrderToInvoiceModel
    {
        public string Voyage { get; set; }

        public decimal PriceNetOut { get; set; }

        public string VehicleRegNumber { get; set; }

        public string VehicleTrailerRegNumber { get; set; }

        public InvoiceCompanyModel Company { get; set; }
    }
}
