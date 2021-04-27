namespace BCKFreightTMS.Web.ViewModels.Invoices
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Orders;

    public class OrderToInvoiceModel : IMapFrom<OrderTo>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Voyage { get; set; }

        public decimal PriceNetOut { get; set; }

        public int CurrencyOutId { get; set; }

        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }

        public string VehicleRegNumber { get; set; }

        public string VehicleTrailerRegNumber { get; set; }

        public string CarrierOrderReferenceNum { get; set; }

        public string OrderOrderFromReferenceNum { get; set; }

        public string ReceiveDate { get; set; }

        public bool IsDocValid { get; set; }

        public bool NoVAT { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public DocumentationInputModel DocumentationRecievedDocumentation { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderTo, OrderToInvoiceModel>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => string.Join(" - ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))));
            NoVATTaxCoutryNames res;
            configuration.CreateMap<OrderTo, OrderToInvoiceModel>().ForMember(x => x.NoVAT, opt =>
                    opt.MapFrom(x => x.OrderActions.Any(a => Enum.TryParse<NoVATTaxCoutryNames>(a.TaxCountry.Name, out res))));
            configuration.CreateMap<OrderTo, OrderToInvoiceModel>().ForMember(x => x.ReceiveDate, opt =>
                    opt.MapFrom(x => x.Order.OrderFrom.ReceiveDate.Value.ToLocalTime().ToShortDateString()));
        }
    }
}
