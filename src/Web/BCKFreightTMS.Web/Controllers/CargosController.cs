namespace BCKFreightTMS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class CargosController : Controller
    {
        private readonly IDeletableEntityRepository<Cargo> cargos;
        private readonly ICargosService cargosService;

        public CargosController(
            IDeletableEntityRepository<Cargo> cargos,
            ICargosService cargosService)
        {
            this.cargos = cargos;
            this.cargosService = cargosService;
        }

        public IActionResult Index()
        {
            var cargos = this.cargosService.GetAll<CargoListViewModel>();
            return this.View(cargos);
        }

        public IActionResult Details(string id)
        {
            var data = new Dictionary<string, string>();
            var cargo = this.cargos.All().FirstOrDefault(c => c.Id == id);
            data.Add("Id", cargo.Id);
            data.Add("Превозвач", cargo.OrderTo?.CarrierOrder?.Company.Name);
            data.Add("Клиент", cargo.OrderTo?.Order?.OrderFrom?.Company.Name);
            data.Add("Направление", string.Join(" - ", cargo.OrderTo?.OrderActions.Select(a => a.Address.City)));
            data.Add("Заявка", cargo.OrderTo?.CarrierOrder?.ReferenceNum);
            data.Add("Дължина", cargo.Lenght.ToString());
            data.Add("Широчина", cargo.Width.ToString());
            data.Add("Височина", cargo.Height.ToString());

            return this.View(data.Where(kv => kv.Value != null).ToDictionary(x => x.Key, y => y.Value));
        }

        public IActionResult AddCargo()
        {
            var model = this.cargosService.LoadInputModel();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCargo(CargoInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = this.cargosService.LoadInputModel(input);
                return this.View(input);
            }

            await this.cargosService.AddCargoAsync(input);

            return this.RedirectToAction(GlobalConstants.Index);
        }
    }
}
