namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Cargos;

    public interface ICargosService
    {
        public IEnumerable<T> GetAll<T>();

        public CargoInputModel LoadInputModel(CargoInputModel model = null);

        public Task<string> AddCargoAsync(CargoInputModel input);
    }
}
