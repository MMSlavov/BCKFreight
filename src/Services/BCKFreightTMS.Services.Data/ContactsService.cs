namespace BCKFreightTMS.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class ContactsService : IContactsService
    {
        private readonly IDeletableEntityRepository<Company> companiesRepository;
        private readonly IDeletableEntityRepository<Person> peopleRepository;

        public ContactsService(
            IDeletableEntityRepository<Company> compRepo,
            IDeletableEntityRepository<Person> peopRepo)
        {
            this.companiesRepository = compRepo;
            this.peopleRepository = peopRepo;
        }

        public async Task<string> AddCompanyAsync(CompanyInputModel input)
        {
            if (this.companiesRepository.AllWithDeleted().Any(c => c.Name == input.Name))
            {
                return null;
            }

            var company = new Company
            {
                Name = input.Name,
                TaxCountry = new TaxCountry { Name = input.TaxCountry, },
                TaxNumber = input.TaxNumber,
                Address = new CompanyAddress
                    {
                    MOLFirstName = input.MOLFirstName,
                    MOLLastName = input.MOLLastName,
                    Address = new Address
                        {
                            StreetLine = input.StreetLine,
                            Area = input.Area,
                            City = input.City,
                            Postcode = input.Postcode,
                            State = input.State,
                        },
                    },
                Comunicators = new Comunicators
                    {
                        Mobile1 = input.Mobile1,
                        Mobile2 = input.Mobile2,
                        Phone1 = input.Phone1,
                        Phone2 = input.Phone2,
                        Email1 = input.Email1,
                        Email2 = input.Email2,
                        Details = input.Details,
                    },
            };

            await this.companiesRepository.AddAsync(company);
            await this.companiesRepository.SaveChangesAsync();
            return company.Id;
        }

        public async Task<string> AddPersonAsync(PersonInputModel input)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<AllContactsViewModel> GetAll()
        {
            var contacts = new List<AllContactsViewModel>();
            contacts.AddRange(this.peopleRepository.All().Select(p => new AllContactsViewModel
            {
                Id = p.Id,
                Name = p.FirstName + " " + p.LastName,
                Type = nameof(Person),
                Contacts = p.Comunicators.Email1,
                Address = p.Company.Address.Address.StreetLine,
            }));

            contacts.AddRange(this.companiesRepository.All().Select(c => new AllContactsViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = nameof(Company),
                Contacts = c.Comunicators.Email1 ?? c.Comunicators.Mobile1,
                Address = c.Address.Address.StreetLine,
            }));

            return contacts;
        }
    }
}
