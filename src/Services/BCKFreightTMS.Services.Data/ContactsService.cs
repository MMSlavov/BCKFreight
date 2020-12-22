namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Http;

    public class ContactsService : IContactsService
    {
        private readonly IDeletableEntityRepository<Company> companiesRepository;
        private readonly IDeletableEntityRepository<Person> peopleRepository;
        private readonly IDeletableEntityRepository<TaxCountry> taxCtrRepo;
        private readonly IDeletableEntityRepository<PersonRole> rolesRepo;

        public ContactsService(
            IDeletableEntityRepository<Company> compRepo,
            IDeletableEntityRepository<Person> peopRepo,
            IDeletableEntityRepository<TaxCountry> taxCtrRepo,
            IDeletableEntityRepository<PersonRole> rolesRepo)
        {
            this.companiesRepository = compRepo;
            this.peopleRepository = peopRepo;
            this.taxCtrRepo = taxCtrRepo;
            this.rolesRepo = rolesRepo;
        }

        public async Task<string> AddCompanyAsync(CompanyInputModel input)
        {
            if (this.companiesRepository.All().Any(c => c.Name == input.Name))
            {
                return null;
            }

            var company = new Company
            {
                Name = input.Name,
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

            var taxCoutry = this.taxCtrRepo.All().FirstOrDefault(c => c.Name == input.TaxCountry);
            if (taxCoutry == null)
            {
                taxCoutry = new TaxCountry { Name = input.TaxCountry };
            }

            company.TaxCountry = taxCoutry;

            await this.companiesRepository.AddAsync(company);
            company.Address.AdminId = company.AdminId;
            company.Address.Address.AdminId = company.AdminId;
            company.Comunicators.AdminId = company.AdminId;
            company.TaxCountry.AdminId = company.AdminId;
            await this.companiesRepository.SaveChangesAsync();
            return company.Id;
        }

        public PersonInputModel GetPersonInputModel(PersonInputModel model = null)
        {
            var viewModel = model ?? new PersonInputModel();
            viewModel.CompanyItems = this.companiesRepository.AllAsNoTracking()
                                                   .Select(c => new System.Collections.Generic.KeyValuePair<string, string>(c.Id, c.Name))
                                                   .ToList();
            viewModel.RoleItems = this.rolesRepo.AllAsNoTracking()
                                       .Select(r => new System.Collections.Generic.KeyValuePair<string, string>(r.Id.ToString(), r.Name))
                                       .ToList();
            return viewModel;
        }

        public async Task<string> AddPersonAsync(PersonInputModel input)
        {
            if (this.peopleRepository.All().Any(p => p.FirstName == input.FirstName && p.LastName == input.LastName))
            {
                return null;
            }

            var person = new Person
            {
                CompanyId = input.CompanyId,
                RoleId = input.RoleId,
                FirstName = input.FirstName,
                LastName = input.LastName,
                BirthDate = input.BirthDate.ToUniversalTime(),
                Comunicators = new Comunicators
                {
                    Mobile1 = input.Comunicators.Mobile1,
                },
            };

            await this.peopleRepository.AddAsync(person);
            person.Comunicators.AdminId = person.AdminId;
            await this.peopleRepository.SaveChangesAsync();
            return person.Id;
        }

        public IEnumerable<AllContactsViewModel> GetAll()
        {
            var contacts = new List<AllContactsViewModel>();
            contacts.AddRange(this.peopleRepository.AllAsNoTracking().Select(p => new AllContactsViewModel
            {
                Id = p.Id,
                Name = p.FirstName + " " + p.LastName,
                Type = nameof(Person),
                Contacts = p.Comunicators.Email1 ?? p.Comunicators.Mobile1,
                Address = p.Company.Address.Address.StreetLine,
            }).ToArray());

            contacts.AddRange(this.companiesRepository.AllAsNoTracking().Select(c => new AllContactsViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = nameof(Company),
                Contacts = c.Comunicators.Email1 ?? c.Comunicators.Mobile1,
                Address = c.Address.Address.StreetLine,
            }).ToArray());

            return contacts;
        }

        public object ProcessDataTableRequest(HttpRequest request)
        {
            var draw = request.Form["draw"].FirstOrDefault();
            var start = request.Form["start"].FirstOrDefault();
            var length = request.Form["length"].FirstOrDefault();
            var sortColumn = request.Form["columns[" + request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var contactsData = this.GetAll().AsQueryable();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                contactsData = contactsData.OrderBy(sortColumn + " " + sortColumnDirection);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                contactsData = contactsData.Where(m => m.Name.Contains(searchValue)
                                            || m.Type.Contains(searchValue)
                                            || m.Contacts.Contains(searchValue)
                                            || m.Address.Contains(searchValue));
            }

            recordsTotal = contactsData.Count();
            var data = contactsData.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return jsonData;
        }
    }
}
