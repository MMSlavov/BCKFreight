namespace BCKFreightTMS.Web.ViewModels.Transactions
{
    using System.Collections.Generic;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AccountingModel
    {
        public IEnumerable<AccountingTypeModel> CreditTypeItems { get; set; }

        public IEnumerable<AccountingTypeModel> DebitTypeItems { get; set; }

        public IEnumerable<SelectListItem> CompanyItems { get; set; }

        public IEnumerable<SelectListItem> BankItems { get; set; }
    }

    public class AccountingTypeModel : IMapFrom<AccountingType>
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
