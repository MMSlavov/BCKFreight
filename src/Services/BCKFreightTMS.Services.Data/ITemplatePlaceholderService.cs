namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using BCKFreightTMS.Web.ViewModels.Orders;

    public interface ITemplatePlaceholderService
    {
        string ReplacePlaceholders(string template, CarrierOrderApplicationModel model);

        Dictionary<string, string> GetAvailablePlaceholders();
    }
}
