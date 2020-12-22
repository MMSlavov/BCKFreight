namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Http;

    public interface IContactsService
    {
        IEnumerable<AllContactsViewModel> GetAll();

        Task<string> AddPersonAsync(PersonInputModel input);

        Task<string> AddCompanyAsync(CompanyInputModel input);

        PersonInputModel GetPersonInputModel(PersonInputModel model = null);

        public object ProcessDataTableRequest(HttpRequest request);
    }
}
