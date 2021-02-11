namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Web.ViewModels.Cargos;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OrderEditInputModel
    {
        public OrderEditInputModel()
        {
            this.Cargo = new CargoInputModel();
            this.Documentation = new DocumentationInputModel();
        }

        public string ReturnUrl { get; set; }

        public string Id { get; set; }

        // [RegularExpression(@"^\d+(\.|,)\d{2}$", ErrorMessage = "Invalid price.")]
        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        public string OrderFromReferenceNum { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal OrderToPriceNetOut { get; set; }

        public int OrderToCurrencyId { get; set; }

        public string OrderToCompanyId { get; set; }

        public string OrderToVehicleId { get; set; }

        public string OrderToDriverId { get; set; }

        public string OrderToContactId { get; set; }

        public CargoInputModel Cargo { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public List<ActionCreateInputModel> OrderActions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionTypeItems { get; set; }

        public IEnumerable<SelectListItem> ContactItems { get; set; }

        public IEnumerable<SelectListItem> DriverItems { get; set; }

        public IEnumerable<SelectListItem> VehicleItems { get; set; }
    }
}
