namespace BCKFreightTMS.Web.Controllers.Api
{
    using System;
    using System.Threading.Tasks;

    using BCKFreightTMS.Services;
    using BCKFreightTMS.Services.Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [IgnoreAntiforgeryToken]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService contactsService;
        private readonly ICompaniesManagerService companiesManager;

        public ContactsController(
            IContactsService contactsService,
            ICompaniesManagerService companiesManager)
        {
            this.contactsService = contactsService;
            this.companiesManager = companiesManager;
        }

        /// <summary>
        /// Get company information by search string (name or UIC)
        /// </summary>
        /// <param name="searchStr">Search string (company name or UIC)</param>
        /// <returns>Company information</returns>
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyInfo([FromQuery] string searchStr = null)
        {
            try
            {
                if (string.IsNullOrEmpty(searchStr))
                {
                    return this.BadRequest(new { error = "Search string is required" });
                }

                var model = await this.companiesManager.GetCompanyAsync(searchStr);

                if (model == null)
                {
                    return this.NotFound(new { error = "Company not found" });
                }

                return this.Ok(model);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, new { error = "An error occurred while retrieving company information" });
            }
        }

        /// <summary>
        /// Get all contacts with DataTables server-side processing
        /// </summary>
        /// <returns>DataTables JSON response</returns>
        [HttpPost("datatable")]
        public IActionResult GetContactsDataTable()
        {
            try
            {
                return this.Ok(this.contactsService.ProcessDataTableRequest(this.Request));
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns>List of contacts</returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var contacts = this.contactsService.GetAll();
                return this.Ok(contacts);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get contacts by company ID
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>List of contacts for the specified company</returns>
        [HttpGet("company/{companyId}")]
        public IActionResult GetContactsByCompany(string companyId)
        {
            try
            {
                var contacts = this.contactsService.GetContacts(companyId);
                return this.Ok(contacts);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }
    }
}
