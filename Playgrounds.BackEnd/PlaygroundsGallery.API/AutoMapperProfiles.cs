using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using System.Linq;

namespace PlaygroundsGallery.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Photo, PhotoDto>()
                .ForMember(p => p.PlaygroundLocationStr, o => o.MapFrom(p => $"{p.Playground.Location.City}, {p.Playground.Location.Country}"))
                .ReverseMap();
            CreateMap<Photo, PhotoToUpdateDto>().ReverseMap();
            CreateMap<PhotoToInsertDto, Photo>();
            CreateMap<PhotoToUploadDto, PhotoToInsertDto>();
            CreateMap<Photo, PhotoInsertedDto>();
            
            CreateMap<Member, MemberLoggedInDto>()
                .ForMember(m => m.ProfilePictureUrl, o => o.MapFrom(m => m.ProfilePictures.FirstOrDefault(x => x.Main).Url));
            CreateMap<Member, MemberToLoginDto>().ReverseMap();
            
            CreateMap<Location, LocationDto>().ReverseMap();
            
            CreateMap<CheckIn, CheckInForCreationDto>().ReverseMap();
            CreateMap<CheckIn, CheckInDto>()
                .ForMember(ch => ch.MemberProfilePictureUrl, o => o.MapFrom(m => m.Member.ProfilePictures.FirstOrDefault(x => x.Main).Url))
                .ReverseMap();

            CreateMap<Playground, PlaygroundDto>()
                .ForMember(p => p.LocationStr, o => o.MapFrom(p => $"{p.Location.City}, {p.Location.Country}"))
                .ForMember(p => p.MainPhotoUrl, o => o.MapFrom(p => p.Photos.FirstOrDefault().Url))
                .ReverseMap();
        }
    }
}