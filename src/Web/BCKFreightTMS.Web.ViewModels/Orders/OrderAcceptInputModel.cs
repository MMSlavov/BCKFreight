namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderAcceptInputModel : IValidatableObject
    {
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "Invalid price.(123.45)")]
        [Range(0, 9999999999.99)]
        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }

        public int DueDaysFrom { get; set; }

        [Required]
        public string CompanyFromId { get; set; }

        public string ContactFromId { get; set; }

        public int LoadingBodyId { get; set; }

        [MaxLength(200)]
        public string CargoName { get; set; }

        public int CargoTypeId { get; set; }

        [Range(0, 9999999999.99)]
        public double Lenght { get; set; }

        [Range(0, 9999999999.99)]
        public double Width { get; set; }

        [Range(0, 9999999999.99)]
        public double Height { get; set; }

        [Range(0, 9999999999.99)]
        public decimal WeightGross { get; set; }

        [Range(0, 9999999999.99)]
        public decimal WeightNet { get; set; }

        [Range(0, 9999999999.99)]
        public decimal Cubature { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string Details { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public List<ActionCreateInputModel> Actions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CargoTypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ActionTypeItems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            return errors;
        }
    }
}
