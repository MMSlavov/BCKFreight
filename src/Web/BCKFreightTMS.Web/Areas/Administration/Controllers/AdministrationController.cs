namespace BCKFreightTMS.Web.Areas.Administration.Controllers
{
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
