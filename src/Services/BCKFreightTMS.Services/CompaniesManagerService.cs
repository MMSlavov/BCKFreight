namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using AngleSharp;
    using AngleSharp.Dom;
    using AngleSharp.Io;
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
            var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
            this.context = BrowsingContext.New(config);
        }

        public IReadOnlyCollection<CompanyInfo> Companies => (IReadOnlyCollection<CompanyInfo>)this.companies;

        public async Task AddJsonCompaniesAsync(string pathJson)
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

        public async Task<CompanyInputModel> GetCompanyAsync(string searchStr)
        {
            var urlSearch = string.Format(BaseUrlSearch, searchStr);
            var document = await this.context.OpenAsync(urlSearch);
            if (document.StatusCode == System.Net.HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Няма намерени резултати"))
            {
                throw new InvalidOperationException("Not found");
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

            var mol = elements["Управители/Съдружници"].Trim();
            var address = elements["Седалище адрес"].Split("  ", StringSplitOptions.RemoveEmptyEntries)[0]
                                                    .Split(", ")
                                                    .ToArray();
            company.Name = elements["Наименование"];
            company.TaxCountry = address[0];
            company.StreetLine = string.Join(", ", address.Skip(1));
            company.TaxNumber = elements["ЕИК/ПИК"];
            company.Details = elements["Предмет на дейност"].Split("\n", StringSplitOptions.RemoveEmptyEntries)[2]
                                                            .TrimStart();
            company.MOLFirstName = Regex.Match(mol, @"(?<=: )([А-Яа-я]+)").Value;
            company.MOLLastName = Regex.Match(mol, @"([А-Яа-я]+)(?= \()").Value;

            return company;
        }

        //public async Task<string> SpeditorNetGetCompanyAsync(string searchStr)
        //{
        //    var cookieValue = "SpeditorLogin=%D1%E2%2E%CF%E5%ED%F7%E5%E2!1611760753; sess=1611760753.706223567839362707";
        //    var request = new DocumentRequest(new Url("https://www3.speditor.net/cgi-bin/info.pl?firm=%27%27%C0%EB%ED%E8%EC%E0%F0%27%27%20%C5%CE%CE%C4"));
        //    request.Headers.Add("Cookie", cookieValue);
        //    var document = await this.context.OpenAsync(request, new System.Threading.CancellationToken());

        //    //this.context.SetCookie(new Url("https://www3.speditor.net"), cookieValue);
        //    //var document = await this.context.OpenAsync("https://www3.speditor.net/cgi-bin/info.pl?firm=%27%27%C0%EB%ED%E8%EC%E0%F0%27%27%20%C5%CE%CE%C4");
        //    //var urlSearch = string.Format(BaseUrlSearch, searchStr);
        //    //Console.WriteLine(document.Body.InnerHtml);
        //    return document.Body.InnerHtml;
        //    //var cookieValue = SetCookie(, "sess=1611750746.280510157839486967");
        //    //var document = await this.context.OpenAsync(urlSearch).
        //    //if (document.StatusCode == System.Net.HttpStatusCode.NotFound ||
        //    //    document.DocumentElement.OuterHtml.Contains("Няма намерени резултати"))
        //    //{
        //    //    throw new InvalidOperationException("Not found");
        //    //}

        //    //var company = new CompanyInputModel();

        //    //var companyPath = document.QuerySelectorAll("table > tbody > tr > td > a").FirstOrDefault().GetAttribute("href");
        //    //var url = string.Format(GlobalConstants.RegistryAgencyUrl, companyPath);

        //    //document = await this.context.OpenAsync(url);
        //}
    }
}
