namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OrderToContactFirstName { get; set; }

        public string OrderToContactLastName { get; set; }

        public string OrderToCompanyName { get; set; }

        public decimal OrderToPriceNetOut { get; set; }

        public int OrderToCurrencyId { get; set; }

        public string OrderFromContactFirstName { get; set; }

        public string OrderFromContactLastName { get; set; }

        public string OrderFromCompanyName { get; set; }

        public decimal OrderFromPriceNetIn { get; set; }

        public int OrderFromCurrencyId { get; set; }

        public string Voyage { get; set; }

        public string StatusName { get; set; }

        public string CargoName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, ListOrderViewModel>()
                .ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => string.Join(" -> ", x.OrderActions.Select(oa => oa.Address.City))));
        }
    }
}
