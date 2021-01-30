namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OrderCreateInputModel
    {
        public string Id { get; set; }

        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.|,)\d{2}$", ErrorMessage = "Invalid price.")]
        [Range(0, 9999999999.99)]
        public decimal PriceNetOut { get; set; }

        public int CurrencyOutId { get; set; }

        public int DueDaysTo { get; set; }

        [Required]
        public string CompanyToId { get; set; }

        public string VehicleId { get; set; }

        public string DriverId { get; set; }

        public string ContactToId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }

        public IEnumerable<SelectListItem> AreasItems { get; set; }
    }
}
