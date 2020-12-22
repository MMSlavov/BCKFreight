namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using System.Collections.Generic;

    using BCKFreightTMS.Data.Common.Models;

    public class SettingsListViewModel
    {
        public IEnumerable<SettingViewModel> Settings { get; set; }
    }
}
