namespace BCKFreightTMS.Web.ViewModels.Settings
{
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class CargoTypeViewModel<T> : IMapFrom<T>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AdminId { get; set; }
    }
}