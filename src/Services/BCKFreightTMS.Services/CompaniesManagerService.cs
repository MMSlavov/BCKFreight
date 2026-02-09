namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using BCKFreightTMS.Common;
    using BCKFreightTMS.Web.ViewModels.Contacts;

    public class CompaniesManagerService : ICompaniesManagerService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private ICollection<CompanyInfo> companies;

        public CompaniesManagerService(IHttpClientFactory httpClientFactory)
        {
            this.companies = new List<CompanyInfo>();
            this.httpClientFactory = httpClientFactory;
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
            try
            {
                return await this.GetCompanyFromBulstatApiAsync(searchStr);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve company information: {ex.Message}", ex);
            }
        }

        private static string TryExtractErrorMessage(string responseContent)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return null;
            }

            try
            {
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("error", out var errorElement))
                {
                    return errorElement.GetString()?.Trim();
                }
            }
            catch (JsonException)
            {
                return null;
            }

            return null;
        }

        private async Task<CompanyInputModel> GetCompanyFromBulstatApiAsync(string uic)
        {
            if (string.IsNullOrWhiteSpace(uic))
            {
                throw new InvalidOperationException("UIC (company registration number) is required.");
            }

            using var httpClient = this.httpClientFactory.CreateClient();

            var cleanUic = uic.StartsWith("BG", StringComparison.OrdinalIgnoreCase) ? uic.Substring(2) : uic;
            var companyUic = cleanUic.All(char.IsDigit)
                ? cleanUic
                : await this.ResolveCompanyUicAsync(uic, httpClient);

            if (string.IsNullOrWhiteSpace(companyUic))
            {
                throw new InvalidOperationException("Company not found in CompanyBook registry.");
            }

            var requestUrl = $"{GlobalConstants.RegistryAgencyUrlSearch}/api/companies/{Uri.EscapeDataString(companyUic)}?with_data=true";

            HttpResponseMessage response = null;
            string responseContent = null;

            for (var attempt = 1; attempt <= 3; attempt++)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Accept.Clear();
                request.Headers.Accept.ParseAdd("application/json");

                response = await httpClient.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    break;
                }

                if (attempt < 3)
                {
                    await Task.Delay(TimeSpan.FromSeconds(attempt));
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = TryExtractErrorMessage(responseContent);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new InvalidOperationException("Access forbidden - the CompanyBook API is temporarily unavailable. Please try again later.");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new InvalidOperationException("Company not found in CompanyBook registry.");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    throw new InvalidOperationException("Rate limit exceeded for CompanyBook API. Please try again later.");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new InvalidOperationException($"CompanyBook API returned 500 after retries. Response: {errorMessage ?? responseContent}");
                }

                throw new InvalidOperationException($"API request failed with status code: {response.StatusCode}. {errorMessage}".Trim());
            }

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                throw new InvalidOperationException("No company data received from CompanyBook API.");
            }

            var company = this.ParseBulstatResponse(responseContent);

            if (company == null)
            {
                throw new InvalidOperationException("Could not parse company information from the API response.");
            }

            return company;
        }

        private async Task<string> ResolveCompanyUicAsync(string searchStr, HttpClient httpClient)
        {
            var requestUrl = $"{GlobalConstants.RegistryAgencyUrlSearch}/api/companies/search?name={Uri.EscapeDataString(searchStr)}&limit=1";
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd("application/json");

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
            {
                return null;
            }

            using var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("results", out var results) || results.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            var firstResult = results.EnumerateArray().FirstOrDefault();
            if (firstResult.ValueKind == JsonValueKind.Undefined)
            {
                return null;
            }

            if (firstResult.TryGetProperty("uic", out var uicElement))
            {
                return uicElement.GetString()?.Trim();
            }

            return null;
        }

        private CompanyInputModel ParseBulstatResponse(string jsonResponse)
        {
            using var jsonDoc = JsonDocument.Parse(jsonResponse);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("error", out var errorElement) && errorElement.ValueKind == JsonValueKind.String)
            {
                return null;
            }

            JsonElement companyElement;
            if (!root.TryGetProperty("company", out companyElement))
            {
                companyElement = root;
            }

            if (companyElement.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            var company = new CompanyInputModel();

            if (companyElement.TryGetProperty("companyName", out var companyNameElement))
            {
                if (companyNameElement.TryGetProperty("name", out var nameElement))
                {
                    company.Name = nameElement.GetString()?.Trim() ?? string.Empty;
                }
            }

            if (string.IsNullOrWhiteSpace(company.Name) && companyElement.TryGetProperty("name", out var nameFallback))
            {
                company.Name = nameFallback.GetString()?.Trim() ?? string.Empty;
            }

            if (companyElement.TryGetProperty("uic", out var uicElement))
            {
                var uicValue = uicElement.GetString()?.Trim();
                if (!string.IsNullOrEmpty(uicValue))
                {
                    company.TaxNumber = uicValue.StartsWith("BG", StringComparison.OrdinalIgnoreCase) ? uicValue : $"BG{uicValue}";
                    company.TaxCountry = "BG";
                }
            }

            if (companyElement.TryGetProperty("seat", out var seatElement))
            {
                this.ParseCompanySeat(seatElement, company);
            }
            else if (companyElement.TryGetProperty("correspondenceSeat", out var correspondenceSeatElement))
            {
                this.ParseCompanySeat(correspondenceSeatElement, company);
            }

            if (companyElement.TryGetProperty("subjectOfActivity", out var activityElement))
            {
                company.Details = activityElement.GetString()?.Trim() ?? string.Empty;
            }

            if (companyElement.TryGetProperty("contacts", out var contactsElement))
            {
                if (contactsElement.TryGetProperty("email", out var emailElement))
                {
                    company.Email1 = emailElement.GetString()?.Trim();
                }

                if (contactsElement.TryGetProperty("phone", out var phoneElement))
                {
                    company.Mobile1 = phoneElement.GetString()?.Trim();
                }
            }

            if (companyElement.TryGetProperty("managers", out var managersElement) && managersElement.ValueKind == JsonValueKind.Array)
            {
                var manager = managersElement.EnumerateArray().FirstOrDefault();
                if (manager.ValueKind != JsonValueKind.Undefined && manager.TryGetProperty("name", out var managerNameElement))
                {
                    this.ParseManagerName(managerNameElement.GetString()?.Trim() ?? string.Empty, company);
                }
            }
            else if (companyElement.TryGetProperty("representatives", out var repsElement) && repsElement.ValueKind == JsonValueKind.Array)
            {
                var representative = repsElement.EnumerateArray().FirstOrDefault();
                if (representative.ValueKind != JsonValueKind.Undefined && representative.TryGetProperty("name", out var repNameElement))
                {
                    this.ParseManagerName(repNameElement.GetString()?.Trim() ?? string.Empty, company);
                }
            }

            return company;
        }

        private void ParseCompanySeat(JsonElement seatElement, CompanyInputModel company)
        {
            if (seatElement.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            if (seatElement.TryGetProperty("country", out var countryElement))
            {
                company.TaxCountry = countryElement.GetString()?.Trim();
            }

            if (seatElement.TryGetProperty("settlement", out var cityElement))
            {
                company.City = cityElement.GetString()?.Trim();
            }

            if (seatElement.TryGetProperty("postCode", out var postCodeElement))
            {
                company.Postcode = postCodeElement.GetString()?.Trim();
            }

            if (seatElement.TryGetProperty("region", out var regionElement))
            {
                company.State = regionElement.GetString()?.Trim();
            }

            if (seatElement.TryGetProperty("area", out var areaElement))
            {
                company.Area = areaElement.GetString()?.Trim();
            }

            var streetLineParts = new List<string>();

            if (seatElement.TryGetProperty("street", out var streetElement))
            {
                var street = streetElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(street))
                {
                    streetLineParts.Add(street);
                }
            }

            if (seatElement.TryGetProperty("streetNumber", out var numberElement))
            {
                var number = numberElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(number))
                {
                    streetLineParts.Add(number);
                }
            }

            if (seatElement.TryGetProperty("block", out var blockElement))
            {
                var block = blockElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(block))
                {
                    streetLineParts.Add($"бл. {block}");
                }
            }

            if (seatElement.TryGetProperty("entrance", out var entranceElement))
            {
                var entrance = entranceElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(entrance))
                {
                    streetLineParts.Add($"вх. {entrance}");
                }
            }

            if (seatElement.TryGetProperty("floor", out var floorElement))
            {
                var floor = floorElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(floor))
                {
                    streetLineParts.Add($"ет. {floor}");
                }
            }

            if (seatElement.TryGetProperty("apartment", out var apartmentElement))
            {
                var apartment = apartmentElement.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(apartment))
                {
                    streetLineParts.Add($"ап. {apartment}");
                }
            }

            if (streetLineParts.Any())
            {
                company.StreetLine = string.Join(" ", streetLineParts);
            }
        }

        private void ParseManagerName(string managerText, CompanyInputModel company)
        {
            if (string.IsNullOrWhiteSpace(managerText))
            {
                return;
            }

            // Bulgarian names typically separated by space
            var nameParts = managerText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length >= 2)
            {
                company.MOLFirstName = nameParts[0];
                company.MOLLastName = string.Join(" ", nameParts.Skip(1));
            }
            else if (nameParts.Length == 1)
            {
                company.MOLFirstName = nameParts[0];
            }
        }
    }
}
