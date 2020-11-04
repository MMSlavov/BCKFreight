namespace BCKFreightTMS.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Common.Models;

    public class TaxCountry : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(90)]
        public string Name { get; set; }
    }
}
