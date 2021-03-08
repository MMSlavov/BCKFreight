namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListOrderViewModel : IMapFrom<Order>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReferenceNum { get; set; }

        public string OrderFromContactFirstName { get; set; }

        public string OrderFromContactLastName { get; set; }

        public string OrderFromCompanyName { get; set; }

        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        public string StatusName { get; set; }

        public string Voyage { get; set; }

        public List<OrderToListModel> OrderTos { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, ListOrderViewModel>()
                .ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => $"<p class='m-0 mt-1'>{string.Join("\n\r", x.OrderTos.Select(o => string.Join(" <i class='fas fa-angle-double-right'></i> ", o.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))))}</p>"));
        }
    }
}
