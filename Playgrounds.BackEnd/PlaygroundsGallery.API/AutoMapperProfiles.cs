using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;

namespace PlaygroundsGallery.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Photo, PhotoToReturnDto>();
            CreateMap<Photo, PhotoForCreationDto>().ReverseMap();
            CreateMap<PhotoForCreationDto, PhotoToReturnDto>();
            CreateMap<Member, MemberDto>();
            CreateMap<Member, MemberToLoginDto>().ReverseMap();
        }
    }
}