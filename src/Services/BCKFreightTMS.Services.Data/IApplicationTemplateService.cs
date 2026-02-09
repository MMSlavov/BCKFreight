namespace BCKFreightTMS.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using BCKFreightTMS.Data.Models;

    public interface IApplicationTemplateService
    {
        Task<ApplicationTemplate> GetByIdAsync(string id);

        Task<ApplicationTemplate> GetDefaultForCompanyAsync(string companyId);

        IQueryable<ApplicationTemplate> GetAllForCompany(string companyId);

        Task<string> CreateAsync(ApplicationTemplate template);

        Task UpdateAsync(ApplicationTemplate template);

        Task DeleteAsync(string id);

        Task<ApplicationTemplate> CloneTemplateAsync(string templateId, string newName);
    }
}
