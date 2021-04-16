namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations;

    public class OrderConfirmReferenceModel
    {
        public string Id { get; set; }

        [Required]
        public string OrderFromReferenceNum { get; set; }
    }
}
