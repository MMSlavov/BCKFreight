namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Services.Messaging;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Http;

    public interface IContactsService
    {
        IEnumerable<AllContactsViewModel> GetAll();

        Task<string> AddPersonAsync(PersonInputModel input);

        Task<string> AddCompanyAsync(CompanyInputModel input);

        PersonInputModel GetPersonInputModel(PersonInputModel model = null);

        public object ProcessDataTableRequest(HttpRequest request);

        public Dictionary<string, string> GetContactDetails(string id);

        public Task<bool> DeleteAsync(string id);

        public Task SendEmailToCompanyAsync(
            string companyId,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
