namespace BCKFreightTMS.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class AddressInputModel : IMapFrom<Address>
    {
        [MaxLength(100)]
        public string StreetLine { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^[A-Z\d-]{2,20}$", ErrorMessage = "Invalid postcode.")]
        public string Postcode { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string State { get; set; }

        [MaxLength(200)]
        public string Area { get; set; }
    }
}
