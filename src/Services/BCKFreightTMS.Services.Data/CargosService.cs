namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Cargos;

    public class CargosService : ICargosService
    {
        private readonly IDeletableEntityRepository<Cargo> cargos;
        private readonly IDeletableEntityRepository<CargoType> types;
        private readonly IDeletableEntityRepository<VehicleLoadingBody> loadingBodies;

        public CargosService(
            IDeletableEntityRepository<Cargo> cargos,
            IDeletableEntityRepository<CargoType> types,
            IDeletableEntityRepository<VehicleLoadingBody> loadingBodies)
        {
            this.cargos = cargos;
            this.types = types;
            this.loadingBodies = loadingBodies;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var cargos = this.cargos.All().To<T>().ToList();
            return cargos;
        }

        public CargoInputModel LoadInputModel(CargoInputModel model = null)
        {
            if (model is null)
            {
                model = new CargoInputModel();
            }

            model.TypeItems = this.types.AllAsNoTracking()
                                       .Select(ct => new System.Collections.Generic.KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                       .ToList();
            model.LoadingBodyItems = this.loadingBodies.AllAsNoTracking()
                                                    .Select(lb => new System.Collections.Generic.KeyValuePair<string, string>(lb.Id.ToString(), lb.Name))
                                                    .ToList();
            return model;
        }

        public async Task<string> AddCargoAsync(CargoInputModel input)
        {
            var cargo = new Cargo
            {
                Name = input.Name,
                TypeId = input.TypeId,
                VehicleTypeId = null,
                LoadingBodyId = input.LoadingBodyId == 0 ? null : input.LoadingBodyId,
                Lenght = input.Lenght,
                Width = input.Width,
                Height = input.Height,
                WeightGross = input.WeightGross,
                WeightNet = input.WeightNet,
                Cubature = input.Cubature,
                Quantity = input.Quantity,
                Details = input.Details,
            };

            await this.cargos.AddAsync(cargo);
            await this.cargos.SaveChangesAsync();

            return cargo.Id;
        }
    }
}
