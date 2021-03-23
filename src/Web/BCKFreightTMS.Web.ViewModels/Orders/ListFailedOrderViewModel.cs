namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListFailedOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OrderToContactFirstName { get; set; }

        public string OrderToContactLastName { get; set; }

        public string OrderToCompanyName { get; set; }

        public decimal OrderToPriceNetOut { get; set; }

        public int OrderToCurrencyId { get; set; }

        public string OrderToReferenceNum { get; set; }

        public string OrderFromContactFirstName { get; set; }

        public string OrderFromContactLastName { get; set; }

        public string OrderFromCompanyName { get; set; }

        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        public string OrderFromReferenceNum { get; set; }

        public string Voyage { get; set; }

        public string CargoName { get; set; }

        public string FailReason { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, ListFailedOrderViewModel>()
                .ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => $"<p class='m-0 mt-1'>{string.Join("<br>", x.OrderTos.Select(o => string.Join(" <i class='fas fa-angle-double-right'></i> ", o.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))))}</p>"));
        }
    }
}
