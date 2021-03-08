namespace BCKFreightTMS.Web
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using BCKFreightTMS.Web.ViewModels.Shared;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<DocumentationInputModel, Documentation>();
            this.CreateMap<Documentation, DocumentationInputModel>();
            this.CreateMap<ActionCreateInputModel, OrderAction>();
            this.CreateMap<OrderAction, ActionCreateInputModel>();
            this.CreateMap<AddressInputModel, Address>();
            this.CreateMap<Address, AddressInputModel>();
            this.CreateMap<Order, OrderCreateInputModel>();
            this.CreateMap<OrderCreateInputModel, Order>();
            this.CreateMap<Order, OrderApplicationModel>();
            this.CreateMap<OrderApplicationModel, Order>();
            this.CreateMap<OrderEditInputModel, Order>();
            this.CreateMap<Order, OrderEditInputModel>();
            this.CreateMap<Order, OrderStatusViewModel>().ForMember(x => x.DriversMobiles, opt =>
                                                        opt.MapFrom(x => x.OrderTos.SelectMany(o => o.Drivers).Select(d =>
                                                        new KeyValuePair<string, string>($"{d.Driver.FirstName} {d.Driver.LastName}", d.Driver.Comunicators.Mobile1))));
            this.CreateMap<CargoInputModel, Cargo>();
            this.CreateMap<Cargo, CargoInputModel>();
            this.CreateMap<OrderAction, ActionApplicationModel>();
            this.CreateMap<OrderAction, ActionStatusInputModel>().ForMember(x => x.IsFinnished, opt =>
                                                                    opt.MapFrom(x => x.IsFinished));
            this.CreateMap<ActionApplicationModel, OrderAction>();
            this.CreateMap<CompanyViewModel, Company>();
            this.CreateMap<Company, CompanyViewModel>();
            this.CreateMap<Order, OrderFailModel>();
            this.CreateMap<OrderFailModel, Order>();
            this.CreateMap<Order, InvoiceInOrderModel>();
            this.CreateMap<Company, InvoiceCompanyModel>();
            this.CreateMap<InvoiceCompanyModel, Company>();
            this.CreateMap<InvoiceIn, InvoiceInModel>();
            this.CreateMap<OrderTo, OrderToModel>().ForMember(x => x.OrderStatusActions, opt =>
                    opt.MapFrom(x => x.OrderActions));
            this.CreateMap<OrderToModel, OrderTo>();
            this.CreateMap<OrderTo, OrderToApplicationModel>();
            this.CreateMap<OrderTo, OrderToListModel>();
            this.CreateMap<OrderTo, OrderToInvoiceModel>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => $"<p class='m-0'>{string.Join(" - ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))}</p>"));
        }
    }
}
