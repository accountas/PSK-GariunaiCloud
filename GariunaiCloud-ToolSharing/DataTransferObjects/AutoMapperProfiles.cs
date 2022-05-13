using AutoMapper;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserInfo>()
            .ReverseMap();
        
        CreateMap<ListingInfo, Listing>();
        CreateMap<Listing, ListingInfo>()
            .ForMember(dto => dto.OwnerUsername, opt
                => opt.MapFrom(src => src.Owner.Username));
        
        CreateMap<NewListingPayload, Listing>()
            .ReverseMap();

        CreateMap<Order, OrderPayload>()
            .ForMember(dto => dto.ListingId, opt
                => opt.MapFrom(src => src.Listing.ListingId))
            .ForMember(dto => dto.PlacerUsername, opt
                => opt.MapFrom(src => src.User.Username));
    }
}