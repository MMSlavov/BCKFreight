namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListFailedOrderToViewModel : IMapFrom<OrderTo>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string CarrierOrderCompanyName { get; set; }

        public decimal PriceNetOut { get; set; }

        public int CurrencyOutId { get; set; }

        public string CarrierOrderReferenceNum { get; set; }

        public string OrderOrderFromContactFirstName { get; set; }

        public string OrderOrderFromContactLastName { get; set; }

        public string OrderOrderFromCompanyName { get; set; }

        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }

        public string OrderOrderFromReferenceNum { get; set; }

        public string Voyage { get; set; }

        public string CargoName { get; set; }

        public string FailReason { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderTo, ListFailedOrderToViewModel>()
                .ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => string.Join(" <i class='fas fa-angle-double-right'></i> ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))));
        }
    }
}
