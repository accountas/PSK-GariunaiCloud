using AutoMapper;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.PresentationLayer.DataTransferObjects;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserInfo>()
            .ReverseMap();
        
        
        CreateMap<ListingInfo, Listing>();
        CreateMap<Listing, ListingInfo>()
            .ForMember(dto => dto.OwnerUsername, opt 
                => opt.MapFrom(src => src.Owner.UserName));
        
        CreateMap<NewListingPayload, Listing>()
            .ReverseMap();
    }
}