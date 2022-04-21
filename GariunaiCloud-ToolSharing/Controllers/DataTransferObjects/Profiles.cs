using AutoMapper;
using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;

public class Profiles : Profile
{
    public Profiles()
    {
        //user
        CreateMap<User, UserPayload>();
        CreateMap<UserPayload, User>();
        
        //listing
        CreateMap<Listing, ListingPayload>()
            .ForMember(dto => dto.OwnerUsername, opt => opt.MapFrom(src => src.Owner.UserName));
        CreateMap<ListingPayload, Listing>();
    }
}