namespace BCKFreightTMS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Contacts;

    public interface ICompaniesManagerService
    {
        public IReadOnlyCollection<CompanyInfo> Companies { get; }

        public Task GetJsonCompaniesAsync(string pathJson);

        public Task<CompanyInputModel> GetCompanyAsync(int uic);
    }
}
