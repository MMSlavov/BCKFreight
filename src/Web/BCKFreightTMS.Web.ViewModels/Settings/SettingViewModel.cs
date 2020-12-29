namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Services.Mapping;

    public class SettingViewModel : IMapFrom<SettingModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AdminId { get; set; }
    }
}
