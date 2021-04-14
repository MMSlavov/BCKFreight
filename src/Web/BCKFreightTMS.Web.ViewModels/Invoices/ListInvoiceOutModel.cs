namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListInvoiceOutModel : IMapFrom<InvoiceOut>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string StatusName { get; set; }

        public string Number { get; set; }

        public string ClientCompanyName { get; set; }

        public string BankDetailsCompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int DueDays { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<InvoiceOut, ListInvoiceOutModel>().ForMember(x => x.ClientCompanyName, opt =>
                                            opt.MapFrom(x => x.OrderTos.First().Order.OrderFrom.Company.Name));
        }
    }
}
