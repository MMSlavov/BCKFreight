namespace BCKFreightTMS.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationTemplateService : IApplicationTemplateService
    {
        private readonly IDeletableEntityRepository<ApplicationTemplate> templateRepository;

        public ApplicationTemplateService(IDeletableEntityRepository<ApplicationTemplate> templateRepository)
        {
            this.templateRepository = templateRepository;
        }

        public async Task<ApplicationTemplate> GetByIdAsync(string id)
        {
            return await this.templateRepository.All()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ApplicationTemplate> GetDefaultForCompanyAsync(string companyId)
        {
            return await this.templateRepository.All()
                .Where(t => t.CompanyId == companyId && t.IsDefault)
                .FirstOrDefaultAsync();
        }

        public IQueryable<ApplicationTemplate> GetAllForCompany(string companyId)
        {
            return this.templateRepository.All()
                .Where(t => t.CompanyId == companyId);
        }

        public async Task<string> CreateAsync(ApplicationTemplate template)
        {
            if (template.IsDefault)
            {
                await this.UnsetDefaultsForCompanyAsync(template.CompanyId);
            }

            await this.templateRepository.AddAsync(template);
            await this.templateRepository.SaveChangesAsync();

            return template.Id;
        }

        public async Task UpdateAsync(ApplicationTemplate template)
        {
            if (template.IsDefault)
            {
                await this.UnsetDefaultsForCompanyAsync(template.CompanyId, template.Id);
            }

            this.templateRepository.Update(template);
            await this.templateRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var template = await this.GetByIdAsync(id);
            if (template != null)
            {
                this.templateRepository.Delete(template);
                await this.templateRepository.SaveChangesAsync();
            }
        }

        public async Task<ApplicationTemplate> CloneTemplateAsync(string templateId, string newName)
        {
            var original = await this.GetByIdAsync(templateId);
            if (original == null)
            {
                return null;
            }

            var clone = new ApplicationTemplate
            {
                CompanyId = original.CompanyId,
                TemplateName = newName,
                HtmlTemplate = original.HtmlTemplate,
                CssStyles = original.CssStyles,
                JavaScript = original.JavaScript,
                Description = $"Cloned from: {original.TemplateName}",
                IsDefault = false,
                Version = 1,
            };

            await this.templateRepository.AddAsync(clone);
            await this.templateRepository.SaveChangesAsync();

            return clone;
        }

        private async Task UnsetDefaultsForCompanyAsync(string companyId, string excludeId = null)
        {
            var defaults = await this.templateRepository.All()
                .Where(t => t.CompanyId == companyId && t.IsDefault && t.Id != excludeId)
                .ToListAsync();

            foreach (var template in defaults)
            {
                template.IsDefault = false;
                this.templateRepository.Update(template);
            }

            if (defaults.Any())
            {
                await this.templateRepository.SaveChangesAsync();
            }
        }
    }
}
