namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class CarrierOrderApplicationModel
    {
        public string Id { get; set; }

        public string ReferenceNum { get; set; }

        public int OrderDueDaysTo { get; set; }

        public bool NoVAT { get; set; }

        public List<OrderToApplicationModel> OrderTos { get; set; }

        public CompanyViewModel OrderCreatorCompany { get; set; }

        public CompanyViewModel Company { get; set; }
    }
}
