using AutoMapper;
using GariunaiCloud_ToolSharing.Controllers.DataTransferObjects;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;

namespace GariunaiCloud_ToolSharing.DataTransferObjects;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        
        CreateMap<User, UserInfo>()
            .ReverseMap();
        
        CreateMap<ListingInfo, Listing>();
        
        CreateMap<Listing, ListingInfo>()
            .ForMember(dto => dto.OwnerUsername, opt
                => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dto => dto.ETag, opt
                => opt.MapFrom(src => Convert.ToHexString(src.Version)));
        
        CreateMap<NewListingPayload, Listing>()
            .ForMember(l => l.Version, opt
                => opt.MapFrom(dto => Convert.FromHexString(dto.ETag)))
            .ReverseMap();

        CreateMap<Order, OrderPayload>()
            .ForMember(dto => dto.ListingId, opt
                => opt.MapFrom(src => src.Listing.ListingId))
            .ForMember(dto => dto.PlacerUsername, opt
                => opt.MapFrom(src => src.User.Username));
    }
}