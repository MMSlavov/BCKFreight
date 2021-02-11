namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderFailModel
    {
        public string Id { get; set; }

        public string ReturnUrl { get; set; }

        public string OrderToReferenceNum { get; set; }

        [Required]
        public string FailReason { get; set; }
    }
}
