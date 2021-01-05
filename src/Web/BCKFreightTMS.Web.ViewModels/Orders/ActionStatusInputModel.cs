namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using AutoMapper;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;

    public class ActionStatusInputModel : IMapFrom<OrderAction>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public bool IsFinnished { get; set; }

        public int NotFinishedReasonId { get; set; }

        public string Details { get; set; }

        public string AddressCity { get; set; }

        public string AddressStreetLine { get; set; }

        public int TypeId { get; set; }

        public bool NoNotes { get; set; }

        public string Notes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderAction, ActionStatusInputModel>()
                .ForMember(x => x.IsFinnished, opt =>
                    opt.MapFrom(x => x.IsFinished));
        }
    }
}
