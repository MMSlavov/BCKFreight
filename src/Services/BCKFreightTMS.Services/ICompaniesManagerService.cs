namespace BCKFreightTMS.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Contacts;

    public interface ICompaniesManagerService
    {
        public IReadOnlyCollection<CompanyInfo> Companies { get; }

        public Task AddJsonCompaniesAsync(string pathJson);

        public Task<CompanyInputModel> GetCompanyAsync(string searchStr);

        // public Task<string> SpeditorNetGetCompanyAsync(string searchStr);
    }
}
