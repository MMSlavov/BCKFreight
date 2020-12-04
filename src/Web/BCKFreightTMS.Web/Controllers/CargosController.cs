namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class CargosController : Controller
    {
        private readonly IDeletableEntityRepository<Cargo> cargos;
        private readonly IDeletableEntityRepository<CargoType> types;

        public CargosController(
            IDeletableEntityRepository<Cargo> cargos,
            IDeletableEntityRepository<CargoType> types)
        {
            this.cargos = cargos;
            this.types = types;
        }

        public IActionResult Index()
        {
            var cargos = this.cargos.All().To<CargoListViewModel>().ToList();
            return this.View(cargos);
        }

        public IActionResult AddCargo()
        {
            var model = new CargoInputModel();
            model.TypeItems = this.types.AllAsNoTracking()
                                       .Select(ct => new System.Collections.Generic.KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                                       .ToList();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCargo(CargoInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.TypeItems = this.types.AllAsNoTracking()
                           .Select(ct => new System.Collections.Generic.KeyValuePair<string, string>(ct.Id.ToString(), ct.Name))
                           .ToList();
                return this.View(input);
            }

            var cargo = new Cargo
            {
                Name = input.Name,
                TypeId = input.TypeId,
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

            return this.RedirectToAction("Index");
        }
    }
}
