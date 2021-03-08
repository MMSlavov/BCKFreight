namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Shared;

    public class ActionCreateInputModel : IMapFrom<OrderAction>
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public AddressInputModel Address { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid datetime.")]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy hh}")]
        public DateTime Until { get; set; }

        public string Details { get; set; }
    }
}
