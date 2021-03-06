﻿namespace BCKFreightTMS.Web
{
    using System;
    using System.Linq;

    using AutoMapper;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using BCKFreightTMS.Web.ViewModels.Comunicators;
    using BCKFreightTMS.Web.ViewModels.Contacts;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using BCKFreightTMS.Web.ViewModels.Shared;
    using BCKFreightTMS.Web.ViewModels.Transactions;

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
            this.CreateMap<Order, OrderStatusViewModel>();
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
            this.CreateMap<Order, OrderConfirmReferenceModel>();
            this.CreateMap<OrderConfirmReferenceModel, Order>();
            this.CreateMap<Company, InvoiceCompanyModel>();
            this.CreateMap<InvoiceCompanyModel, Company>();
            this.CreateMap<Company, CompanyEditModel>();
            this.CreateMap<CompanyEditModel, Company>();
            this.CreateMap<Person, PersonInputModel>();
            this.CreateMap<PersonInputModel, Person>();
            this.CreateMap<ComunicatorsInputModel, Comunicators>();
            this.CreateMap<Comunicators, ComunicatorsInputModel>();
            this.CreateMap<InvoiceIn, InvoiceInModel>();
            this.CreateMap<InvoiceIn, InvoiceInEditModel>();
            this.CreateMap<InvoiceOut, InvoiceOutEditModel>();
            this.CreateMap<InvoiceOutInputModel, InvoiceNoteOutInputModel>();
            this.CreateMap<InvoiceInEditModel, InvoiceIn>().ForPath(x => x.Id, x => x.Ignore())
                                                        .ForPath(x => x.Status.Name, x => x.Ignore());
            this.CreateMap<InvoiceOutEditModel, InvoiceOut>().ForPath(x => x.Id, x => x.Ignore())
                                                            .ForPath(x => x.Status.Name, x => x.Ignore());
            this.CreateMap<InvoiceInModel, InvoiceIn>().ForMember(x => x.Id, opt => opt.Ignore());
            this.CreateMap<OrderTo, OrderToModel>().ForMember(x => x.OrderStatusActions, opt =>
                    opt.MapFrom(x => x.OrderActions));
            this.CreateMap<OrderToModel, OrderTo>();
            NoVATTaxCoutryNames res;
            this.CreateMap<OrderTo, OrderToApplicationModel>().ForMember(x => x.NoVAT, opt =>
                    opt.MapFrom(x => x.OrderActions.Any(a => Enum.TryParse<NoVATTaxCoutryNames>(a.TaxCountry.Name, out res))));
            this.CreateMap<OrderTo, OrderToListModel>();
            this.CreateMap<OrderTo, OrderToInvoiceModel>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => string.Join(" - ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))))
                                                          .ForMember(x => x.NoVAT, opt =>
                    opt.MapFrom(x => x.OrderActions.Any(a => Enum.TryParse<NoVATTaxCoutryNames>(a.TaxCountry.Name, out res))))
                                                          .ForMember(x => x.ReceiveDate, opt =>
                    opt.MapFrom(x => x.Order.OrderFrom.ReceiveDate == null ? string.Empty : x.Order.OrderFrom.ReceiveDate.Value.ToLocalTime().ToShortDateString()));
            this.CreateMap<OrderToInvoiceModel, OrderTo>();
            this.CreateMap<CarrierOrder, CarrierOrderApplicationModel>();
            this.CreateMap<CarrierOrder, CarrierOrderListModel>();
            this.CreateMap<OrderTo, InvoiceInInputModel>();
            this.CreateMap<OrderTo, InvoiceNoteOutInputModel>();
            this.CreateMap<OrderTo, InvoiceOutInputModel>();
            this.CreateMap<InvoiceOut, InvoiceOutModel>();
            this.CreateMap<NoteInfoModel, NoteInfo>();
            this.CreateMap<NoteInfo, NoteInfoModel>();
            this.CreateMap<OrderTo, ListFailedOrderToViewModel>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => string.Join(" <i class='fas fa-angle-double-right'></i> ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))));
            this.CreateMap<BankDetailsModel, BankDetails>();
            this.CreateMap<InvoiceOut, InvoiceModel>();
            this.CreateMap<OrderTo, ListOrderToViewModel>().ForMember(x => x.Voyage, opt =>
                    opt.MapFrom(x => $"<p class='m-0 mt-1'>{$"{string.Join(" <i class='fas fa-angle-double-right'></i> ", x.OrderActions.OrderBy(oa => oa.TypeId).Select(oa => oa.Address.City))}<br><b>{x.Vehicle.RegNumber}"}{(x.Vehicle.Trailer != null ? $" / {x.Vehicle.Trailer.RegNumber}" : string.Empty)}</b></p>"));
            this.CreateMap<UNCRStatementModel, BankStatementModel>().ForMember(x => x.Movements, opt =>
                    opt.MapFrom(x => x.Items));
            this.CreateMap<UNCRAccountMovement, BankAccountMovementModel>().ForMember(x => x.Date, opt =>
                                                opt.MapFrom(x => x.PaymentDateTime))
                                                                           .ForMember(x => x.Reason, opt =>
                                                opt.MapFrom(x => $"{x.Reason}\n({(x.NarrativeI02 == null ? x.Narrative : x.NarrativeI02)})"));
            this.CreateMap<DSKStatementModel, BankStatementModel>().ForMember(x => x.Movements, opt =>
                                                opt.MapFrom(x => x.AccountMovements));
            this.CreateMap<AccountMovement, BankAccountMovementModel>().ForMember(x => x.Date, opt =>
                                                opt.MapFrom(x => x.AccountingDate));
        }
    }
}
