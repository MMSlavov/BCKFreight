namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Contacts;

    public interface IContactsService
    {
        IEnumerable<AllContactsViewModel> GetAll();

        Task<string> AddPersonAsync(PersonInputModel input);

        Task<string> AddCompanyAsync(CompanyInputModel input);
    }
}
