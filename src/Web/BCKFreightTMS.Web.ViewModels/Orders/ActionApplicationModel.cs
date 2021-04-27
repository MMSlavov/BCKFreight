namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Shared;

    public class ActionApplicationModel : IMapFrom<OrderAction>
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public AddressInputModel Address { get; set; }

        public DateTime Until { get; set; }

        public string Details { get; set; }
    }
}
