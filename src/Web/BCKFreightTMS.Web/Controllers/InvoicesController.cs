namespace BCKFreightTMS.Web.Controllers
{
    using System.Linq;

    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User")]
    public class InvoicesController : Controller
    {
        private readonly IDeletableEntityRepository<InvoiceIn> invoiceIns;

        public InvoicesController(IDeletableEntityRepository<InvoiceIn> invoiceIns)
        {
            this.invoiceIns = invoiceIns;
        }

        public IActionResult Filed()
        {
            var invoices = this.invoiceIns.All().Where(i => i.Status.Name != InvoiceStatusNames.AwaitingApproval.ToString())
                                              .To<ListInvoiceInModel>()
                                              .ToList();
            return this.View(invoices);
        }

        public IActionResult Incomplete()
        {
            var invoices = this.invoiceIns.All().Where(i => i.Status.Name == InvoiceStatusNames.AwaitingApproval.ToString())
                                              .To<ListInvoiceInModel>()
                                              .ToList();
            return this.View(invoices);
        }
    }
}
