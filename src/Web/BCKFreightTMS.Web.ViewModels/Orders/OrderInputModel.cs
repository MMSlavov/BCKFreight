namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputModel
    {
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "Invalid price.(123.45)")]
        [Range(0, 9999999999.99)]
        public decimal PriceNetIn { get; set; }

        public int DueDaysFrom { get; set; }

        [Required]
        public string CompanyFromId { get; set; }

        public string ContactFromId { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "Invalid price.(123.45)")]
        [Range(0, 9999999999.99)]
        public decimal PriceNetOut { get; set; }

        public int DueDaysTo { get; set; }

        [Required]
        public string CompanyToId { get; set; }

        public string VehicleId { get; set; }

        public string DriverId { get; set; }

        public string ContactToId { get; set; }

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

        [MaxLength(100)]
        public string LoadingStreetLine { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^[A-Z\d-]{2,20}$", ErrorMessage = "Invalid postcode.")]
        public string LoadingPostcode { get; set; }

        [MaxLength(200)]
        public string LoadingCity { get; set; }

        [MaxLength(200)]
        public string LoadingState { get; set; }

        [MaxLength(200)]
        public string LoadingArea { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy hh}")]
        public DateTime LoadingUntil { get; set; }

        public string LoadingDetails { get; set; }

        [MaxLength(100)]
        public string UnloadingStreetLine { get; set; }

        [MaxLength(20)]
        public string UnloadingPostcode { get; set; }

        [MaxLength(200)]
        public string UnloadingCity { get; set; }

        [MaxLength(200)]
        public string UnloadingState { get; set; }

        [MaxLength(200)]
        public string UnloadingArea { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid datetime.")]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy hh}")]
        public DateTime UnloadingUntil { get; set; }

        public string UnloadingDetails { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CargoTypeItems { get; set; }
    }
}
