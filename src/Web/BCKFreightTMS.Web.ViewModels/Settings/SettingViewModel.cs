namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using AutoMapper;
    using BCKFreightTMS.Data.Common.Models;
    using BCKFreightTMS.Services.Mapping;

    public class SettingViewModel : IMapFrom<SettingModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AdminId { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap(typeof(SettingModel), typeof(SettingViewModel<SettingModel>));
        //}
    }
}
