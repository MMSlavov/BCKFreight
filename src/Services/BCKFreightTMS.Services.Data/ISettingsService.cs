namespace BCKFreightTMS.Services.Data
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Settings;

    public interface ISettingsService
    {
        public Task AddPersonRoleAsync(SettingInputModel input);

        public Task DeletePersonRoleAsync(int id);

        public Task AddCargoTypeAsync(SettingInputModel input);

        public Task DeleteCargoTypeAsync(int id);

        public Task AddLoadingBodyAsync(SettingInputModel input);

        public Task DeleteLoadingBodyAsync(int id);
    }
}
