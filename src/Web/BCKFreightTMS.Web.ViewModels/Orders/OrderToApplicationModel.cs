namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using BCKFreightTMS.Web.ViewModels.Cargos;

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
