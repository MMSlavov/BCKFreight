namespace BCKFreightTMS.Web
{
    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Cargos;
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
            this.CreateMap<CargoInputModel, Cargo>();
            this.CreateMap<Cargo, CargoInputModel>();
        }
    }
}
