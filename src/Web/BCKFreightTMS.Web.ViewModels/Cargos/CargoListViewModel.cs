namespace BCKFreightTMS.Web.ViewModels.Cargos
{
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CargoListViewModel : IMapFrom<Cargo>
    {
        public string Id { get; set; }

        public string TypeName { get; set; }

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
    }
}
