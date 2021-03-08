namespace BCKFreightTMS.Web.ViewModels.Cargos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CargoInputModel : IMapFrom<Cargo>
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [Range(0.0, 9999999999.99)]
        public double Lenght { get; set; }

        [Range(0.0, 9999999999.99)]
        public double Width { get; set; }

        [Range(0.0, 9999999999.99)]
        public double Height { get; set; }

        [Range(0.0, 9999999999.99)]
        public decimal WeightGross { get; set; }

        [Range(0.0, 9999999999.99)]
        public decimal WeightNet { get; set; }

        [Range(0.0, 9999999999.99)]
        public decimal Cubature { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [MinLength(5)]
        public string Details { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public int LoadingBodyId { get; set; }

        public string LoadingBodyName { get; set; }

        public string VehicleRequirements { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TypeItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> LoadingBodyItems { get; set; }
    }
}
