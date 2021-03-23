namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using System.Collections.Generic;

    public class OrderToApplicationModel
    {
        public decimal PriceNetOut { get; set; }

        public string CurrencyName { get; set; }

        public string VehicleRegNumber { get; set; }

        public string VehicleTrailerRegNumber { get; set; }

        public string CargoVehicleRequirements { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public List<ActionCreateInputModel> OrderActions { get; set; }

        public CargoInputModel Cargo { get; set; }
    }
}
