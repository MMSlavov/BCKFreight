namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Web.ViewModels.Shared;

    public class ActionCreateInputModel
    {
        public int TypeId { get; set; }

        public AddressInputModel Address { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid datetime.")]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy hh}")]
        public DateTime Until { get; set; }

        public string Details { get; set; }
    }
}
