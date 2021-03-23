namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ListInvoiceOrderTo : IMapFrom<OrderTo>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Voyage { get; set; }

        public decimal PriceNetOut { get; set; }

        public string VehicleRegNumber { get; set; }

        public string VehicleTrailerRegNumber { get; set; }

        public string CarrierOrderReferenceNum { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderTo, ListInvoiceOrderTo>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => $"<p class='m-0'>{string.Join(" -> ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))}</p>"));
        }
    }
}
