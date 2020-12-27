namespace BCKFreightTMS.Web.Controllers
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Services.Data;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class CargosController : Controller
    {
        private readonly ICargosService cargosService;

        public CargosController(ICargosService cargosService)
        {
            this.cargosService = cargosService;
        }

        public IActionResult Index()
        {
            var cargos = this.cargosService.GetAll<CargoListViewModel>();
            return this.View(cargos);
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
