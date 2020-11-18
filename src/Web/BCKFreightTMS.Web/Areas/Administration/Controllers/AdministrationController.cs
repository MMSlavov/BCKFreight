namespace BCKFreightTMS.Web.Areas.Administration.Controllers
{
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {

    }
}
