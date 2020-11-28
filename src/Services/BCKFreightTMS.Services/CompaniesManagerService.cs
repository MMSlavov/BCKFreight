namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class CompaniesManagerService : ICompaniesManagerService
    {
        private const string BaseUrlSearch = GlobalConstants.RegistryAgencyUrlSearch;
        private readonly IBrowsingContext context;
        private ICollection<CompanyInfo> companies;

        public CompaniesManagerService()
        {
            this.companies = new List<CompanyInfo>();
            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
        }

        public IReadOnlyCollection<CompanyInfo> Companies => (IReadOnlyCollection<CompanyInfo>)this.companies;

        public async Task GetJsonCompaniesAsync(string pathJson)
        {
            using (FileStream fs = File.OpenRead(pathJson))
            {
                using var jsonDoc = await JsonDocument.ParseAsync(fs);
                var root = jsonDoc.RootElement;
                var deedsElement = root.GetProperty("Message")
                                     .EnumerateArray()
                                     .FirstOrDefault()
                                     .GetProperty("Body")
                                     .EnumerateArray()
                                     .FirstOrDefault()
                                     .GetProperty("Deeds")
                                     .EnumerateArray()
                                     .FirstOrDefault()
                                     .GetProperty("Deed")
                                     .EnumerateArray()
                                     .Select(e => e.GetProperty("$"));

                foreach (var companyE in deedsElement)
                {
                    var company = new CompanyInfo
                    {
                        DeedStatus = companyE.GetProperty("DeedStatus").ToString(),
                        CompanyName = companyE.GetProperty("CompanyName").ToString(),
                        GUID = companyE.GetProperty("GUID").ToString(),
                        UIC = companyE.GetProperty("UIC").ToString(),
                        LegalForm = companyE.GetProperty("LegalForm").ToString(),
                    };
                    this.companies.Add(company);
                }
            }
        }

        public async Task<CompanyInputModel> GetCompanyAsync(int uic)
        {
            var urlSearch = string.Format(BaseUrlSearch, uic);

            var document = await this.context.OpenAsync(urlSearch);
            if (document.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException();
            }

            var company = new CompanyInputModel();

            var companyPath = document.QuerySelectorAll("table > tbody > tr > td > a").FirstOrDefault().GetAttribute("href");
            var url = string.Format(GlobalConstants.RegistryAgencyUrl, companyPath);

            document = await this.context.OpenAsync(url);
            var data = new Stack<IElement>(document.All.Where(e => e.ClassList.Any(c => c == "col-sm-3" || c == "col-sm-9")));
            var elements = new Dictionary<string, string>();
            while (data.Any())
            {
                var value = data.Pop().TextContent.Trim();
                var key = data.Pop().TextContent.Trim();
                elements.Add(key, value);
            }

            var mol = elements["Управители/Съдружници"].Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[0]
                                                       .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            company.Name = elements["Наименование"];
            company.StreetLine = elements["Седалище адрес"].Split("  ", StringSplitOptions.RemoveEmptyEntries)[0];
            company.TaxNumber = elements["ЕИК/ПИК"];
            company.Details = elements["Предмет на дейност"].Split("\n", StringSplitOptions.RemoveEmptyEntries)[2]
                                                            .TrimStart();
            company.MOLFirstName = mol[1];
            company.MOLLastName = mol[3];

            return company;
        }
    }
}
