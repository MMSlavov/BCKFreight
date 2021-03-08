namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using BCKFreightTMS.Web.ViewModels.Cargos;
    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class OrderApplicationModel
    {
        public string ReferenceNum { get; set; }

        public List<OrderToApplicationModel> OrderTos { get; set; }

        public CompanyViewModel CreatorCompany { get; set; }

        //public List<ActionApplicationModel> OrderActions { get; set; }

        //public CargoInputModel Cargo { get; set; }

        //public DocumentationInputModel Documentation { get; set; }
    }
}
