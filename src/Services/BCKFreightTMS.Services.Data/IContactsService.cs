namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Services.Messaging;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IContactsService
    {
        IEnumerable<AllContactsViewModel> GetAll();

        Task<string> AddPersonAsync(PersonInputModel input);

        Task<string> AddCompanyAsync(CompanyInputModel input);

        public CompanyEditModel LoadEditCompanyModel(string companyId);

        public Task<string> EditCompanyAsync(CompanyEditModel input);

        Task<int> AddBankDetails(BankDetailsModel input);

        public IEnumerable<SelectListItem> GetBankDetails(string companyId);

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
