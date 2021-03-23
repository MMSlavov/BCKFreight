namespace BCKFreightTMS.Web.ViewModels.Shared
{
    using System.Collections.Generic;

    public class ApplicationModel
    {
        public string OrderId { get; set; }

        public List<ApplicationPreview> AppPreviews { get; set; }

        public string ReturnUrl { get; set; }
    }
}
