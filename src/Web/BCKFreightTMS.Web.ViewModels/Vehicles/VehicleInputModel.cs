namespace BCKFreightTMS.Web.ViewModels.Vehicles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleInputModel
    {
        [Required]
        public int TypeId { get; set; }

        public int LoadingBodyId { get; set; }

        public string DriverId { get; set; }

        public string CompanyId { get; set; }

        public string TrailerId { get; set; }

        [Required]

        // [RegularExpression(@"^[A-Z]{1,2}[\d]{4}[A-Z]{2}$", ErrorMessage = "Invalid registration number format.")]
        public string RegNumber { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Details { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CompanyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DriverItems { get; set; }
    }
}
