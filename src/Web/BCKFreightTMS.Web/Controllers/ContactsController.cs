namespace BCKFreightTMS.Web.Controllers
{
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class ContactsController : Controller
    {
        public IActionResult All()
        {
            var contacts = new List<AllContactsViewModel>();
            return View(contacts);
        }

        public IActionResult Create()
        {

        }
    }
}
