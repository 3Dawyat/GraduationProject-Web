namespace BL.Models.CustomModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<InvoiceItem, TbOrdersItems>()
            .ForMember(dest => dest.ItemUnitId, opt => opt.MapFrom(src => src.ItemId))
           .ReverseMap();

            CreateMap<InvoiceHead, TbOrders>()
                .ForMember(dest => dest.TbOrdersItems, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<ItemModel, VwItemsWithUnits>()
             .ForMember(dest => dest.ItemUnitId, opt => opt.MapFrom(src => src.ItemId))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ItemName))
             .ReverseMap();

            CreateMap<DetailsItemModel, VwItemsWithUnits>()
           .ForMember(dest => dest.ItemUnitId, opt => opt.MapFrom(src => src.ItemId))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ItemName))
           .ReverseMap();

            CreateMap<RevealModel, TbOrders>()
            .ForMember(dest => dest.TbOrdersItems, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<RegisterModel,ApplicationUser>().ReverseMap();



            CreateMap<IdAndNameModel, TbCategories>().ReverseMap();
        }
    }
}
