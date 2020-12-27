namespace BCKFreightTMS.Web.ViewModels.Cargos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CargoInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Range(0.0, double.MaxValue)]
        public double Lenght { get; set; }

        [Range(0.0, double.MaxValue)]
        public double Width { get; set; }

        [Range(0.0, double.MaxValue)]
        public double Height { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal WeightGross { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal WeightNet { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal Cubature { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [MinLength(5)]
        public string Details { get; set; }

        public int TypeId { get; set; }

        public int LoadingBodyId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }
    }
}
