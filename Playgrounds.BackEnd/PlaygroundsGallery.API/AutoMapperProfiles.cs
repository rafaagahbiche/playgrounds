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
            CreateMap<Photo, PhotoToReturnDto>().ReverseMap();
            CreateMap<Photo, PhotoForCreationDto>().ReverseMap();
            CreateMap<PhotoForCreationDto, PhotoToReturnDto>();
            CreateMap<Member, MemberDto>();
            CreateMap<Member, MemberToLoginDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<CheckIn, CheckInForCreationDto>().ReverseMap();
            CreateMap<CheckIn, CheckInDto>()
                .ReverseMap();
            CreateMap<Playground, PlaygroundDto>()
                .ForMember(p => p.LocationStr, o => o.MapFrom(p => $"{p.Location.City}, {p.Location.Country}"))
                .ForMember(p => p.MainPhotoUrl, o => o.MapFrom(p => p.Photos.FirstOrDefault().Url))
                .ReverseMap();
        }
    }
}