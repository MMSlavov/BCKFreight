namespace BCKFreightTMS.Web.Controllers
{
    using System.Globalization;
    using System.Threading;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [Authorize]
    public class BaseController : Controller
    {
    }
}
