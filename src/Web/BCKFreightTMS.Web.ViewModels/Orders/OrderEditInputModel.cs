namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderEditInputModel
    {
        public string ReturnUrl { get; set; }

        public string Id { get; set; }

        // [RegularExpression(@"^\d+(\.|,)\d{2}$", ErrorMessage = "Invalid price.")]
        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        public string OrderFromReferenceNum { get; set; }

        public List<OrderToModel> OrderTos { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionTypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CargoTypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }
    }
}
